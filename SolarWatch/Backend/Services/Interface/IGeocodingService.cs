using SolarWatch.Backend.Models;

namespace SolarWatch.Services;

public interface IGeocodingService
{
    public Task<City> GetCityByName(string city);
}