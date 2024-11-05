namespace SolarWatch.Services;

public class SunriseSunsetRepository : ISunriseSunsetRepository
{
    private SolarWatchApiContext dbContext;

    public SunriseSunsetRepository(SolarWatchApiContext context)
    {
        dbContext = context;
    }



    public SunriseAndSunset? GetSunriseAndSunset(City city, DateOnly date)
    {
        return dbContext.SunriseAndSunsets.FirstOrDefault(s =>
            s.City == city && s.Date == date);
    }

    public void AddSunriseAndSunset(SunriseAndSunset sunriseAndSunset)
    {
        dbContext.Add(sunriseAndSunset);
        dbContext.SaveChanges();
    }
}