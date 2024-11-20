using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Services;

public class UserHistoryEntry
{
    [Key]
    public int Id { get; set; }
    public string AspNetUserId { get; set; }
    public int CityId { get; init; }
    public City City {get ; set; }
    public DateTime CreatedAt { get; set; }
}