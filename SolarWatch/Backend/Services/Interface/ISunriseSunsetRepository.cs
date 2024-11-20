namespace SolarWatch.Services;

public interface ISunriseSunsetRepository
{
    SunriseAndSunset? GetSunriseAndSunset(City city, DateOnly date);
    void AddSunriseAndSunset(SunriseAndSunset sunriseAndSunset);

    void AddUserHistory(UserHistoryEntry userHistoryEntry);
    List<string> GetUserHistory(string userId);

}