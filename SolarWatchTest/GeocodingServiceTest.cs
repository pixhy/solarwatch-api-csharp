using Moq;
using SolarWatch.Services;

namespace SolarWatchTest;

public class Tests
{
    private Mock<IWebDownloader> _webdownloader;
    private GeocodingService _geocodingService;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<ICityRepository> _cityRepository;
    
    [SetUp]
    public void Setup()
    {
        _cityRepository = new Mock<ICityRepository>();
        _webdownloader = new Mock<IWebDownloader>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _geocodingService = new GeocodingService("", _webdownloader.Object, _unitOfWork.Object);
        
        _unitOfWork.Setup(x => x.Cities).Returns(_cityRepository.Object);
        
    }

    [Test]
    public async Task CoordinateTest()
    {
        
       _webdownloader.Setup(x => x.GetStringByUrl(It.IsAny<string>()))
            .ReturnsAsync("[{\"name\": \"Test\", \"lat\": 47.4979937,\n    \"lon\": 19.0403594, \"country\": \"valami\", \"state\": \"valami\"}]");

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