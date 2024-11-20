using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch.Backend.Authentication;
using SolarWatch.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

AddServices();
AddAuthentication();
AddDbContext();
AddIdentity();
AddSwagger();

var app = builder.Build();

using var scope = app.Services.CreateScope(); // AuthenticationSeeder is a scoped service, therefore we need a scope instance to access it
var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
authenticationSeeder.AddRoles();
authenticationSeeder.AddAdmin();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


void AddServices()
{
    builder.Services.AddScoped<IGeocodingService>(provider =>
    {
        var configuration = provider.GetRequiredService<IConfiguration>();
        var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
        var key =
            configuration.GetValue<string>("OpenWeatherMapAPIKey");
        return new GeocodingService(key, new WebDownloader(), unitOfWork);
    });

    builder.Services.AddScoped<ISunriseSunsetService>(provider =>
    {
        var geocodingService = provider.GetRequiredService<IGeocodingService>();
        var  unitOfWork = provider.GetRequiredService<IUnitOfWork>();
        return new SunriseSunsetService(geocodingService, new WebDownloader(), unitOfWork);
    });
    builder.Services.AddScoped<ISunriseSunsetRepository, SunriseSunsetRepository>();
    builder.Services.AddScoped<ICityRepository, CityRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<AuthenticationSeeder>();
}


void AddAuthentication()
{
    var jwtSettings = builder.Configuration.GetSection("Authentication");

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["ValidIssuer"],
                ValidAudience = jwtSettings["ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["IssuerSigningKey"])
                ),
            };
        });
}

void AddDbContext()
{
    
    builder.Services.AddDbContext<SolarWatchApiContext>(options =>
    {
        options.UseSqlServer(
            "Server=mssql,1433;Database=Solarwatch;User Id=sa;Password=Cicacica!;Encrypt=false;");
    
    });
}

void AddIdentity()
{
    builder.Services
        .AddIdentityCore<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<SolarWatchApiContext>();
}

void AddSwagger()
{
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "SolarWatch API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });
}

public partial class Program { }
