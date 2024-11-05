using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarWatch.Services;

public class SunriseAndSunset
{
    //[Key]
    public int Id { get; init; }
    
    public int CityId { get; init; }
    public City City {get ; init; }
    public DateOnly Date { get; init; }
    public TimeOnly Sunrise { get; init; }
    public TimeOnly Sunset { get; init; }
}