using System.ComponentModel.DataAnnotations;

namespace SolarWatch.Backend.Models;

public class City
{
    [Key]
    public int Id { get; init; }
    public required string Name { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string? State { get; init; }
    public required string Country { get; init; }
}