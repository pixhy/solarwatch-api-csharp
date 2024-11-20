using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
    
    public void AddUserHistory(UserHistoryEntry userHistoryEntry)
    {
        dbContext.Add(userHistoryEntry);
        dbContext.SaveChanges();
    }

    public List<string> GetUserHistory(string userId)
    {
        var userHistoryEntries = dbContext.UserHistoryEntries
            .Where(e => e.AspNetUserId == userId)
            .Join(dbContext.Cities, entry => entry.CityId, city => city.Id, (entry, city) => new{city.Name, entry.CreatedAt, entry.CityId})
            .GroupBy(e => e.CityId)
            .Select(e => e.First())
            .Skip(1)
            .ToList();
        
        return new List<string>(userHistoryEntries.AsEnumerable().OrderByDescending(e => e.CreatedAt).Take(10).Select(arg => arg.Name));
    }
}