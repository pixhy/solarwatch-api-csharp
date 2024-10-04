using System.Net;
using System.Text.Json;

namespace SolarWatch.Services;

public class GeocodingService(string apikey, IWebDownloader webDownloader) : IGeocodingService
{
    public async Task<Coordinate> GetCoordinatesByCity(string city)
    {
        var url =
            $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={apikey}";

        var cityData = webDownloader.GetStringByUrl(url);

        return ProcessCityData(await cityData);
    }

    private Coordinate ProcessCityData(string data)
    {
        
        JsonDocument json = JsonDocument.Parse(data);
        if (json.RootElement.GetArrayLength() == 0)
        {
            throw new CityNotFoundException();
        }
        JsonElement lat = json.RootElement[0].GetProperty("lat");
        JsonElement lon = json.RootElement[0].GetProperty("lon");

        return new Coordinate(lat.GetDouble(), lon.GetDouble());
    }
}

public class CityNotFoundException : Exception
{
}