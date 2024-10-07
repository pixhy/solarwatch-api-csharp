namespace SolarWatch.Services;

public interface IGeocodingService
{
    public Task<City> GetCityByName(string city);
}