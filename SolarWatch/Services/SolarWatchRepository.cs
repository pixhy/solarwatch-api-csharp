namespace SolarWatch.Services;

public class SolarWatchRepository : ISolarWatchRepository
{
    private SolarWatchApiContext dbContext;

    public SolarWatchRepository(SolarWatchApiContext context)
    {
        dbContext = context;
    }

    public IEnumerable<City> GetAllCities()
    {
        return dbContext.Cities.ToList();
    }

    public City? GetCityByName(string name)
    {
        return dbContext.Cities.FirstOrDefault(c => c.Name == name);
    }

    public void AddCity(City city)
    {
        dbContext.Add(city);
        dbContext.SaveChanges();
    }

    public void DeleteCity(City city)
    {
        dbContext.Remove(city);
        dbContext.SaveChanges();
    }

    public void UpdateCity(City city)
    {  
        dbContext.Update(city);
        dbContext.SaveChanges();
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