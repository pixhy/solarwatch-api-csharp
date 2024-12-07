using System.ComponentModel.DataAnnotations;
using SolarWatch.Services;

namespace SolarWatch.Backend.Models;

public class UserHistoryEntry
{
    [Key]
    public int Id { get; init; }
    public required string AspNetUserId { get; init; }
    public int CityId { get; init; }
    public required City City {get ; init; }
    public DateTime CreatedAt { get; init; }
}