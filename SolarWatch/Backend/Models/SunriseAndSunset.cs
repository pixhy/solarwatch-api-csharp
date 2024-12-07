using SolarWatch.Services;

namespace SolarWatch.Backend.Models;

public class SunriseAndSunset
{
    public int Id { get; init; }
    public int CityId { get; init; }
    public required City City {get ; init; }
    public DateOnly Date { get; init; }
    public TimeOnly Sunrise { get; init; }
    public TimeOnly Sunset { get; init; }
}