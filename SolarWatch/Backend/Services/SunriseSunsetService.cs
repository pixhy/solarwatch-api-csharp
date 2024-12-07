using System.Globalization;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using SolarWatch.Backend.Models;

namespace SolarWatch.Services;

public class SunriseSunsetService(IGeocodingService geocodingService, IWebDownloader webDownloader, IUnitOfWork unitOfWork) : ISunriseSunsetService
{
    public async Task<SunriseAndSunset> GetSunriseAndSunset(string city, DateOnly date)
    {
        City cityData = await geocodingService.GetCityByName(city);

        SunriseAndSunset? citySunriseSunset =
            unitOfWork.SunriseSunsets.GetSunriseAndSunset(cityData, date);
        if (citySunriseSunset != null)
        {
            return citySunriseSunset;
        }
        
        var lat = cityData.Latitude.ToString(CultureInfo.InvariantCulture);
        var lng = cityData.Longitude.ToString(CultureInfo.InvariantCulture);
        var dateString = date.ToString("yyyy-MM-dd");
        
        string url =
             $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lng}&date={dateString}&tzid=CET";
        
        var sunriseSunsetData = webDownloader.GetStringByUrl(url);

        var sunriseAndSunset = ProcessSunriseAndSunsetData(await sunriseSunsetData);

        var result = new SunriseAndSunset()
        {
            Sunrise = sunriseAndSunset.sunrise,
            Sunset = sunriseAndSunset.sunset,
            City = cityData,
            Date = date
        };
        unitOfWork.SunriseSunsets.AddSunriseAndSunset(result);
        return result;
    }
    
    private (TimeOnly sunrise, TimeOnly sunset) ProcessSunriseAndSunsetData(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement result = json.RootElement.GetProperty("results");
        JsonElement sunrise = result.GetProperty("sunrise");
        JsonElement sunset = result.GetProperty("sunset");
        
        return (
            TimeOnly.Parse(sunrise.GetString()!),
            TimeOnly.Parse(sunset.GetString()!)
        );
    }
}