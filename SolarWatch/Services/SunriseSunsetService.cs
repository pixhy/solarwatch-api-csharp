using System.Globalization;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace SolarWatch.Services;

public class SunriseSunsetService(IGeocodingService geocodingService, IWebDownloader webDownloader)
{
    private IGeocodingService _geocodingService = geocodingService;
    private IWebDownloader _webDownloader = webDownloader;


    public SunriseAndSunset GetSunriseAndSunset(string city, DateOnly date)
    {
        Coordinate coordinates = _geocodingService.GetCoordinatesByCity(city);
        var lat = coordinates.Latitude.ToString(CultureInfo.InvariantCulture);
        var lng = coordinates.Longitude.ToString(CultureInfo.InvariantCulture);
        var dateString = date.ToString("yyyy-MM-dd");
        
        string url =
             $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lng}&date={dateString}";
        
        var sunriseSunsetData = _webDownloader.GetStringByUrl(url);

        return ProcessSunriseAndSunsetData(sunriseSunsetData);
    }
    
    private SunriseAndSunset ProcessSunriseAndSunsetData(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement result = json.RootElement.GetProperty("results");
        JsonElement sunrise = result.GetProperty("sunrise");
        JsonElement sunset = result.GetProperty("sunset");
        
        return new SunriseAndSunset(TimeOnly.Parse(sunrise.GetString()!), TimeOnly.Parse(sunset.GetString()!));
    }
}