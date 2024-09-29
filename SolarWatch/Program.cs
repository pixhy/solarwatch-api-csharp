using SolarWatch.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<GeocodingService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var key =
        configuration.GetValue<string>("OpenWeatherMapAPIKey");
    return new GeocodingService(key, new WebDownloader());
});

builder.Services.AddScoped<SunriseSunsetService>(provider =>
{
    var geocodingService = provider.GetRequiredService<IGeocodingService>();
    return new SunriseSunsetService(geocodingService, new WebDownloader());
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