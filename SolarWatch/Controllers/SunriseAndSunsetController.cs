using Microsoft.AspNetCore.Mvc;
using SolarWatch.Services;

namespace SolarWatch.Controllers;


[ApiController]
[Route ("api/v1/[controller]")]
public class SunriseAndSunsetController(
   ISunriseSunsetService sunriseSunsetService)
   : ControllerBase
{
   private ISunriseSunsetService _sunriseSunsetService = sunriseSunsetService;

   [HttpGet]
   public async Task<ActionResult<SunriseAndSunset>> GetSunriseAndSunset(string city, string date)
   {
      DateOnly dateObject;
      if (!DateOnly.TryParse(date, out dateObject))
      {
         return BadRequest("Wrong date");
      }
      
      try
      {
         SunriseAndSunset? sunriseAndSunset = await _sunriseSunsetService.GetSunriseAndSunset(city, dateObject);
         
         return Ok(sunriseAndSunset);
      }
      catch (CityNotFoundException)
      {
         return NotFound("City not found");
      }
   }
}