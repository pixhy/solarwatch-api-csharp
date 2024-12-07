using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using SolarWatch.Backend.Models;
using SolarWatch.Controllers;
using SolarWatch.Services;

namespace SolarWatchTest;

public class SunriseAndSunsetControllerTest
{
    private Mock<ISunriseSunsetService> _sunriseSunsetService;
    private SunriseAndSunsetController _sunriseAndSunsetController;
    private Mock<IUnitOfWork> _unitOfWork;

    [SetUp]

    public void SetUp()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _sunriseSunsetService = new Mock<ISunriseSunsetService>();
        _sunriseAndSunsetController = new SunriseAndSunsetController(_sunriseSunsetService.Object, _unitOfWork.Object);
        
        
        var identity = new GenericIdentity("username");

        var principal = new ClaimsPrincipal(identity);

        var context = new DefaultHttpContext()
        {
            User = principal
        };

        _sunriseAndSunsetController.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };
    }

    [Test]
    public async Task GetSunriseAndSunsetTest()
    {
        
        
        string city = "test";
        string date = "2024-10-09";
        var dateObject = DateOnly.Parse(date);
        var expectedResult = new SunriseAndSunset()
        {
            City = new City()
            {
                Country = "test", Id = 1, Longitude = 19.0403594,
                Latitude = 47.4979937, Name = "test", State = "test"
            },
            CityId = 1, Sunrise = new TimeOnly(04, 39, 15),
            Sunset = new TimeOnly(16, 28, 44), Date = dateObject, Id = 1
        };
        _sunriseSunsetService.Setup(x => x.GetSunriseAndSunset(city, dateObject))
            .ReturnsAsync(expectedResult);

        var result = await
            _sunriseAndSunsetController.GetSunriseAndSunset(city, date);
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(((OkObjectResult)result.Result!).Value,
            Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task CityNotFoundThrowsException()
    {
        string date = "2024-10-09";
        var dateObject = DateOnly.Parse(date);
        string city = "test";
        
         _sunriseSunsetService
            .Setup(x => x.GetSunriseAndSunset(city, dateObject))
            .Throws<CityNotFoundException>();

         var result =
             await _sunriseAndSunsetController.GetSunriseAndSunset(city, date);
         Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
         Assert.That(((NotFoundObjectResult)result.Result!).Value,
             Is.EqualTo("City not found"));
    }
}