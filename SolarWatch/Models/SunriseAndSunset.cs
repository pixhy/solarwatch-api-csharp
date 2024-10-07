using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarWatch.Services;

public class SunriseAndSunset(TimeOnly sunrise, TimeOnly sunset)
{
    [Key]
    public int Id { get; init; }
    
    public int CityId { get; set; }
    public City City {get ; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Sunrise { get; set; } = sunrise;
    public TimeOnly Sunset { get; set; } = sunset;
}