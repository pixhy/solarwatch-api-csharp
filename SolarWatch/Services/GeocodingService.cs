using System.Net;
using System.Text.Json;

namespace SolarWatch.Services;

public class GeocodingService(string apikey, IWebDownloader webDownloader, IUnitOfWork unitOfWork) : IGeocodingService
{
    public async Task<City> GetCityByName(string city)
    {
        var cityObj = unitOfWork.Cities.GetCityByName(city);
        if (cityObj != null)
        {
            return cityObj;
        }
        var url =
            $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={apikey}";

        var cityData = webDownloader.GetStringByUrl(url);

        var newCity = ProcessCityData(await cityData);
        unitOfWork.Cities.AddCity(newCity);
        return newCity;
    }

    private City ProcessCityData(string data)
    {
        
        JsonDocument json = JsonDocument.Parse(data);
        if (json.RootElement.GetArrayLength() == 0)
        {
            throw new CityNotFoundException();
        }

        JsonElement name = json.RootElement[0].GetProperty("name");
        JsonElement lat = json.RootElement[0].GetProperty("lat");
        JsonElement lon = json.RootElement[0].GetProperty("lon");
        JsonElement country = json.RootElement[0].GetProperty("country");

        string? state = null;
        if (json.RootElement[0].TryGetProperty("state", out JsonElement stateElement))
        {
            state = stateElement.GetString()!;
        }
        return new City()
        {
            
            Name = name.GetString()!,
            Latitude = lat.GetDouble(),
            Longitude = lon.GetDouble(),
            Country = country.GetString()!,
            State = state
        };
    }
}

public class CityNotFoundException : Exception
{
}