namespace SolarWatch.Backend.Models;

public class Forecast
{
    public Dictionary<string, object[]> ForecastDictionary { get; set; } = new Dictionary<string, object[]>();
}