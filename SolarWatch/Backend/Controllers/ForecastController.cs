using Microsoft.AspNetCore.Mvc;
using SolarWatch.Backend.Models;
using SolarWatch.Services;

namespace SolarWatch.Controllers;

[ApiController]
[Route ("api/v1/[controller]")]

public class ForecastController(IForecastService _forecastService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Forecast>> GetForecast(string name)
    {
        var result = await _forecastService.GetForecastByName(name);
        
        return Ok(result);
    }
}