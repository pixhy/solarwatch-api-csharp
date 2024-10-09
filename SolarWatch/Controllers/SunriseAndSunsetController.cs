using Microsoft.AspNetCore.Mvc;
using SolarWatch.Services;

namespace SolarWatch.Controllers;


[ApiController]
[Route ("api/v1/[controller]")]
public class SunriseAndSunsetController(
   ISunriseSunsetService sunriseSunsetService, ISunriseSunsetRepository sunriseSunsetRepository)
   : ControllerBase
{
   private ISunriseSunsetService _sunriseSunsetService = sunriseSunsetService;

   private ISunriseSunsetRepository _sunriseSunsetRepository =
      sunriseSunsetRepository;

   [HttpGet]
   public async Task<IActionResult> GetSunriseAndSunset(string city, string date)
   {
      DateOnly dateObject;
      if (!DateOnly.TryParse(date, out dateObject))
      {
         return BadRequest("Wrong date");
      }

      SunriseAndSunset? sunriseAndSunset = null;
      if (sunriseAndSunset == null)
      {
         try
         {
            sunriseAndSunset = await _sunriseSunsetService.GetSunriseAndSunset(city, dateObject);
            
         }
         catch (CityNotFoundException)
         {
            return NotFound("City not found");
         }
      }
      return Ok(sunriseAndSunset);
   }
}