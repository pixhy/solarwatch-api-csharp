using Moq;
using SolarWatch.Services;

namespace SolarWatchTest;

public class Tests
{
    private Mock<IWebDownloader> _webdownloader;
    private GeocodingService _geocodingService;
    private Mock<IUnitOfWork> _unitOfWork;
    
    [SetUp]
    public void Setup()
    {
        _webdownloader = new Mock<IWebDownloader>();
        _geocodingService = new GeocodingService("", _webdownloader.Object, _unitOfWork.Object);
    }

    [Test]
    public async Task CoordinateTest()
    {
        _webdownloader.Setup(x => x.GetStringByUrl(It.IsAny<string>()))
            .ReturnsAsync("[{\"lat\": 47.4979937,\n    \"lon\": 19.0403594}]");

        var result = await _geocodingService.GetCityByName("Test");
        Assert.That(result.Longitude, Is.EqualTo(19.0403594).Within(0.000001));
        Assert.That(result.Latitude, Is.EqualTo(47.4979937).Within(0.000001));
    }
    [Test]
    public void ExceptionWhenCityNotFound()
    {
        _webdownloader.Setup(x => x.GetStringByUrl(It.IsAny<string>()))
            .ReturnsAsync("[]");
        
        Assert.ThrowsAsync<CityNotFoundException>( async () => await _geocodingService.GetCityByName("Test"));
    }
}