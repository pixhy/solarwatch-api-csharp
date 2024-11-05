namespace SolarWatch.Services;

public interface ISunriseSunsetRepository
{
    SunriseAndSunset? GetSunriseAndSunset(City city, DateOnly date);
    void AddSunriseAndSunset(SunriseAndSunset sunriseAndSunset);

}