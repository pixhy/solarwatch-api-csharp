namespace SolarWatch.Services;

public interface IForecastService
{
    Task<Forecast?> GetForecastByName(string name);
}