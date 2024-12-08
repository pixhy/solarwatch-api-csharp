using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatch.Backend.Authentication;
using SolarWatch.Backend.Models;
using SolarWatch.Services;

namespace SolarWatchIntegrationTest;

public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    
    private readonly string _dbName = Guid.NewGuid().ToString();
    private static string _accessToken = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //Get the previous DbContextOptions registrations 
            var solarWatchDbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SolarWatchApiContext>));
            
            //Remove the previous DbContextOptions registrations
            if (solarWatchDbContextDescriptor != null)
            {
                services.Remove(solarWatchDbContextDescriptor);
            }
            
            //Add new DbContextOptions for our two contexts, this time with inmemory db 
            services.AddDbContext<SolarWatchApiContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            
            //We will need to initialize our in memory databases. 
            //Since DbContexts are scoped services, we create a scope
            using var scope = services.BuildServiceProvider().CreateScope();
            
            var scopedServices = scope.ServiceProvider;
            //We use this scope to request the registered dbcontexts, and initialize the schemas
            var solarContext = scopedServices.GetRequiredService<SolarWatchApiContext>();
            solarContext.Database.EnsureDeleted();
            solarContext.Database.EnsureCreated();

            
            //Here we could do more initializing if we wished (e.g. adding admin user)
            var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
            authenticationSeeder.AddRoles();
            authenticationSeeder.AddAdmin();
            
            var userManager = scopedServices.GetRequiredService<UserManager<IdentityUser>>();
            var tokenService = scopedServices.GetRequiredService<ITokenService>();
            var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();
            SeedUsers(userManager, tokenService, roleManager).Wait();
        });
        
        async Task SeedUsers(UserManager<IdentityUser> userManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
        {
            var user = new IdentityUser { UserName = "test_user", Email = "test@example.com" };
            await userManager.CreateAsync(user, "Password123!");
            var roles = roleManager.Roles.ToList();
            _accessToken = tokenService.CreateToken(user, roles[0].Name!);
        }
    }
    [Collection("IntegrationTests")] //this is to avoid problems with tests running in parallel
    public class MyControllerIntegrationTest
    {
        private readonly HttpClient _client;
        const string BudapestSunriseSunsetUrl = "api/v1/SunriseAndSunset?city=Budapest&date=2024-11-05";
    
        public MyControllerIntegrationTest()
        {
            var app = new SolarWatchWebApplicationFactory();
            _client = app.CreateClient();

        }

        private async Task<HttpResponseMessage> SendRequest(string url, string? accessToken)
        {

            using var requestMessage =
                new HttpRequestMessage(HttpMethod.Get, url);
            if (accessToken != null)
            {
                requestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await _client.SendAsync(requestMessage);
            
            response.EnsureSuccessStatusCode();
            return response;
        }

        [Fact]
        public async Task TestSunriseAndSunsetData()
        {
            var response = await SendRequest(BudapestSunriseSunsetUrl, null);

            var result = new SunriseAndSunset(){Id = 1, CityId = 1, City = new City(){Country = "HU", Id = 1, Name = "Budapest", Longitude = 19.0403594, Latitude = 47.4979937, State = null}, Sunset = TimeOnly.Parse("16:22:07"), Sunrise = TimeOnly.Parse("06:32:45"), Date = DateOnly.Parse("2024-11-05")};
            
            var data = await response.Content.ReadFromJsonAsync<SunriseAndSunset>();
            Assert.NotNull(data);
            if (data != null)
            {
                Assert.Equal(result.Id, data.Id);
                Assert.Equal(result.CityId, data.CityId);
                Assert.Equal(result.Date, data.Date);
                Assert.Equal(result.Sunrise, data.Sunrise);
                Assert.Equal(result.Sunset, data.Sunset);
                Assert.Equal(result.City.Id, data.City.Id);
                Assert.Equal(result.City.Name, data.City.Name);
                Assert.Equal(result.City.State, data.City.State);
            }

        }

        [Fact]
        public async Task TestUserHistory()
        {
            var citySearchResponse = await SendRequest(BudapestSunriseSunsetUrl, _accessToken);
            var searchResult = await citySearchResponse.Content.ReadFromJsonAsync<SunriseAndSunset>();
            Assert.NotNull(searchResult);
            
            var historyResponse = await SendRequest("/api/v1/SunriseAndSunset/UserHistory", _accessToken);
            var history = await historyResponse.Content.ReadFromJsonAsync<List<string>>();
            Assert.NotNull(history);
            Assert.NotEmpty(history);
            
            if (searchResult != null && history != null)
            { 
                Assert.Equal(searchResult.City.Name, history[0]);
            }
        }
    }

}