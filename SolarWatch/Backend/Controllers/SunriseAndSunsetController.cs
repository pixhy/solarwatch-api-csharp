using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Backend.Models;
using SolarWatch.Services;

namespace SolarWatch.Controllers;


[ApiController]
[Route ("api/v1/[controller]")]
public class SunriseAndSunsetController(
   ISunriseSunsetService sunriseSunsetService, IUnitOfWork unitOfWork)
   : ControllerBase
{
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
         SunriseAndSunset? sunriseAndSunset = await sunriseSunsetService.GetSunriseAndSunset(city, dateObject);

         var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

         if (user != null)
         {
            UserHistoryEntry userHistoryEntry = new UserHistoryEntry(){AspNetUserId = user, City = sunriseAndSunset.City, CityId = sunriseAndSunset.CityId, CreatedAt = DateTime.Now};

            unitOfWork.SunriseSunsets.AddUserHistory(userHistoryEntry);
         }
         unitOfWork.SaveChanges();
         return Ok(sunriseAndSunset);
      }
      catch (CityNotFoundException)
      {
         return NotFound("City not found");
      }
   }

   [Authorize]
   [HttpGet("UserHistory")]
   public ActionResult<List<string>> GetUserHistory()
   {
      var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
      
      var userHistory = unitOfWork.SunriseSunsets.GetUserHistory(user!);
      
      unitOfWork.SaveChanges();
      
      return Ok(userHistory);
   }
}