using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatch.Services;

namespace SolarWatchIntegrationTest;

public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    
    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //Get the previous DbContextOptions registrations 
            var solarWatchDbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SolarWatchApiContext>));

            
            //Remove the previous DbContextOptions registrations
            services.Remove(solarWatchDbContextDescriptor);

            
            //Add new DbContextOptions for our two contexts, this time with inmemory db 
            services.AddDbContext<SolarWatchApiContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });

            
            //We will need to initialize our in memory databases. 
            //Since DbContexts are scoped services, we create a scope
            using var scope = services.BuildServiceProvider().CreateScope();
            
            //We use this scope to request the registered dbcontexts, and initialize the schemas
            var solarContext = scope.ServiceProvider.GetRequiredService<SolarWatchApiContext>();
            solarContext.Database.EnsureDeleted();
            solarContext.Database.EnsureCreated();

            //Here we could do more initializing if we wished (e.g. adding admin user)
        });
    }
    [Collection("IntegrationTests")] //this is to avoid problems with tests running in parallel
    public class MyControllerIntegrationTest
    {
        private readonly SolarWatchWebApplicationFactory _app;
        private readonly HttpClient _client;
    
        public MyControllerIntegrationTest()
        {
            _app = new SolarWatchWebApplicationFactory();
            _client = _app.CreateClient();
        }

        [Fact]
        public async Task TestEndPoint()
        {
            var response = await _client.GetAsync("api/v1/SunriseAndSunset?city=Budapest&date=2024-11-05");

            response.EnsureSuccessStatusCode();


            var result = new SunriseAndSunset(){Id = 1, CityId = 1, City = new City(){Country = "HU", Id = 1, Name = "Budapest", Longitude = 19.0403594, Latitude = 47.4979937, State = null}, Sunset = TimeOnly.Parse("16:22:07"), Sunrise = TimeOnly.Parse("06:32:45"), Date = DateOnly.Parse("2024-11-05")};
            
            
            var data = await response.Content.ReadFromJsonAsync<SunriseAndSunset>();
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

}