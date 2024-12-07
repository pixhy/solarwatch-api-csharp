using System.Text.Json;
using SolarWatch.Backend.Models;

namespace SolarWatch.Services;

public class ForecastService(string? apikey, IWebDownloader webDownloader) : IForecastService
{
    
    
    public async Task<Forecast?> GetForecastByName(string name)
    {
        string url = $"http://api.weatherapi.com/v1/forecast.json?key={apikey}&q={name}&days=1&aqi=no&alerts=no";
        
        var forecastData = webDownloader.GetStringByUrl(url);

        var newForecast = ProcessForecastData(await forecastData);

        return newForecast;
    }


    private Forecast ProcessForecastData(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);

        JsonElement forecast = json.RootElement.GetProperty("forecast");
        var forecastDays = forecast.GetProperty("forecastday");
        var day = forecastDays[0];
        var hours = day.GetProperty("hour");
        
       


        var forecastResult = new Forecast();
        
        
        for (var i = 0; i < hours.GetArrayLength(); i++)
        {
            var hour = hours[i];
            var time = hour.GetProperty("time");
            var temp = hour.GetProperty("temp_c");
            
            var condition = hour.GetProperty("condition");
            var icon = condition.GetProperty("icon");
            
            forecastResult.ForecastDictionary.Add(time.GetString()!, [temp.GetDouble(), icon.GetString()!]);
        }

        return forecastResult;
    }
}