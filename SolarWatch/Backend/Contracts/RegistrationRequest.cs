using System.ComponentModel.DataAnnotations;

namespace SolarWatch.Backend.Contracts;

public record RegistrationRequest(
    [Required]string Email, 
    [Required]string Username, 
    [Required]string Password);
