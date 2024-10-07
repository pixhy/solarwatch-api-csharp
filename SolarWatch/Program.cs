using Microsoft.EntityFrameworkCore;
using SolarWatch.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IGeocodingService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var solarWatchRepository = provider.GetRequiredService<ISolarWatchRepository>();
    var key =
        configuration.GetValue<string>("OpenWeatherMapAPIKey");
    return new GeocodingService(key, new WebDownloader(), solarWatchRepository);
});

builder.Services.AddScoped<ISunriseSunsetService>(provider =>
{
    var geocodingService = provider.GetRequiredService<IGeocodingService>();
    var solarWatchRepository = provider.GetRequiredService<ISolarWatchRepository>();
    return new SunriseSunsetService(geocodingService, new WebDownloader(), solarWatchRepository);
});
builder.Services.AddScoped<ISolarWatchRepository, SolarWatchRepository>();
builder.Services.AddDbContext<SolarWatchApiContext>(options =>
{
    options.UseSqlServer(
        "Server=localhost,1433;Database=Solarwatch;User Id=sa;Password=Cicacica!;Encrypt=false;");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();