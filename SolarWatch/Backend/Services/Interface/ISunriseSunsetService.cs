using SolarWatch.Backend.Models;

namespace SolarWatch.Services;

public interface ISunriseSunsetService
{
    public Task<SunriseAndSunset> GetSunriseAndSunset(string city, DateOnly date);
}