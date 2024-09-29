using Moq;
using SolarWatch.Services;

namespace SolarWatchTest;

public class SunriseSunsetServiceTest
{
    
    private Mock<IGeocodingService> _geocodingServiceMock;
    private Mock<IWebDownloader> _webdownloader;
    private SunriseSunsetService _sunriseSunsetService;
    
    [SetUp]
    public void Setup()
    {
        _geocodingServiceMock = new Mock<IGeocodingService>();
        _webdownloader = new Mock<IWebDownloader>();
        _sunriseSunsetService = new SunriseSunsetService(_geocodingServiceMock.Object, _webdownloader.Object);
    }

    [Test]
    public void GetSunriseAndSunsetTest()
    {
        _geocodingServiceMock.Setup(x => x.GetCoordinatesByCity("test"))
            .Returns(new Coordinate(47.4979937, 19.0403594));
        _webdownloader.Setup(x => x.GetStringByUrl(It.IsAny<string>())).Returns(
            """{"results":{"sunrise": "04:39:15", "sunset": "16:28:44"}}""");

        var result =
            _sunriseSunsetService.GetSunriseAndSunset("test",
                DateOnly.Parse("2024-09-29"));
        Assert.That(new TimeOnly(4, 39, 15), Is.EqualTo(result.Sunrise));
        Assert.That(new TimeOnly(16, 28, 44), Is.EqualTo(result.Sunset));
    }
}