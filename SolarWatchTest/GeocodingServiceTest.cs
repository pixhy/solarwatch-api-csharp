using Moq;
using SolarWatch.Services;

namespace SolarWatchTest;

public class Tests
{
    private Mock<IWebDownloader> _webdownloader;
    private GeocodingService _geocodingService;
    
    [SetUp]
    public void Setup()
    {
        _webdownloader = new Mock<IWebDownloader>();
        _geocodingService = new GeocodingService("", _webdownloader.Object);
    }

    [Test]
    public void CoordinateTest()
    {
        _webdownloader.Setup(x => x.GetStringByUrl(It.IsAny<string>()))
            .Returns("[{\"lat\": 47.4979937,\n    \"lon\": 19.0403594}]");

        var result = _geocodingService.GetCoordinatesByCity("Test");
        Assert.That(result.Longitude, Is.EqualTo(19.0403594).Within(0.000001));
        Assert.That(result.Latitude, Is.EqualTo(47.4979937).Within(0.000001));
    }
    [Test]
    public void ExceptionWhenCityNotFound()
    {
        _webdownloader.Setup(x => x.GetStringByUrl(It.IsAny<string>()))
            .Returns("[]");
        
        Assert.Throws<CityNotFoundException>(() => _geocodingService.GetCoordinatesByCity("Test"));
    }
}