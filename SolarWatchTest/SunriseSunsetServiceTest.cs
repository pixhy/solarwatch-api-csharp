using Moq;
using SolarWatch.Services;

namespace SolarWatchTest;

public class SunriseSunsetServiceTest
{
    
    private Mock<IGeocodingService> _geocodingServiceMock;
    private Mock<IWebDownloader> _webdownloader;
    private SunriseSunsetService _sunriseSunsetService;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<ISunriseSunsetRepository> _sunriseSunsetRepository;
    
    [SetUp]
    public void Setup()
    {
        _geocodingServiceMock = new Mock<IGeocodingService>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _webdownloader = new Mock<IWebDownloader>();
        _sunriseSunsetService = new SunriseSunsetService(_geocodingServiceMock.Object, _webdownloader.Object, _unitOfWork.Object);

        _sunriseSunsetRepository = new Mock<ISunriseSunsetRepository>();
        _unitOfWork.Setup(x => x.SunriseSunsets).Returns(_sunriseSunsetRepository.Object);
    }

    [Test]
    public async Task GetSunriseAndSunsetTest()
    {
        _geocodingServiceMock.Setup(x => x.GetCityByName("test"))
            .ReturnsAsync(new City(){Latitude = 47.4979937,Longitude = 19.0403594, Id = 1, Country = "test", Name = "test", State = "test"});
        _webdownloader.Setup(x => x.GetStringByUrl(It.IsAny<string>())).ReturnsAsync(
            """{"results":{"sunrise": "04:39:15", "sunset": "16:28:44"}}""");

        var result = await
            _sunriseSunsetService.GetSunriseAndSunset("test",
                DateOnly.Parse("2024-09-29"));
        Assert.That(new TimeOnly(4, 39, 15), Is.EqualTo(result.Sunrise));
        Assert.That(new TimeOnly(16, 28, 44), Is.EqualTo(result.Sunset));
    }
}