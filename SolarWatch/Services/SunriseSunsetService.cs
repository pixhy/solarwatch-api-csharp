using System.Globalization;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace SolarWatch.Services;

public class SunriseSunsetService(IGeocodingService geocodingService, IWebDownloader webDownloader, ISolarWatchRepository solarWatchRepository) : ISunriseSunsetService
{
    public async Task<SunriseAndSunset> GetSunriseAndSunset(string city, DateOnly date)
    {
        City cityData = await geocodingService.GetCityByName(city);

        SunriseAndSunset? citySunriseSunset =
            solarWatchRepository.GetSunriseAndSunset(cityData, date);
        if (citySunriseSunset != null)
        {
            return citySunriseSunset;
        }
        
        var lat = cityData.Latitude.ToString(CultureInfo.InvariantCulture);
        var lng = cityData.Longitude.ToString(CultureInfo.InvariantCulture);
        var dateString = date.ToString("yyyy-MM-dd");
        
        string url =
             $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lng}&date={dateString}";
        
        var sunriseSunsetData = webDownloader.GetStringByUrl(url);

        var result = ProcessSunriseAndSunsetData(await sunriseSunsetData);
        
        solarWatchRepository.AddSunriseAndSunset(result);
        return result;
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