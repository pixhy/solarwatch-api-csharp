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
   public async Task<IActionResult> GetSunriseAndSunset(string city, string date)
   {
      DateOnly dateObject;
      if (!DateOnly.TryParse(date, out dateObject))
      {
         return BadRequest("Wrong date");
      }

      try
      {
         var sunriseAndSunset = await 
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