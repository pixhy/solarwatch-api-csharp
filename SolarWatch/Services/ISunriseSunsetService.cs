namespace SolarWatch.Services;

public interface ISunriseSunsetService
{
    public SunriseAndSunset GetSunriseAndSunset(string city, DateOnly date);
}