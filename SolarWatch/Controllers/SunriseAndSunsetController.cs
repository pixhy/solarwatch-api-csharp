using Microsoft.AspNetCore.Mvc;
using SolarWatch.Services;

namespace SolarWatch.Controllers;


[ApiController]
[Route ("api/v1/[controller]")]
public class SunriseAndSunsetController(
   SunriseSunsetService sunriseSunsetService)
   : ControllerBase
{
   private SunriseSunsetService _sunriseSunsetService = sunriseSunsetService;

   [HttpGet]
   public IActionResult GetSunriseAndSunset(string city, string date)
   {
      DateOnly dateObject;
      if (!DateOnly.TryParse(date, out dateObject))
      {
         return BadRequest("Wrong date");
      }

      try
      {
         var sunriseAndSunset =
            _sunriseSunsetService.GetSunriseAndSunset(city,
               DateOnly.Parse(date));

         return Ok(sunriseAndSunset);
      }
      catch (CityNotFoundException)
      {
         return NotFound("City not found");
      }
   }
}