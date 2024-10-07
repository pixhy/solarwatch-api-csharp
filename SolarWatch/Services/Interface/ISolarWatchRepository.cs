namespace SolarWatch.Services;

public interface ISolarWatchRepository
{
    IEnumerable<City> GetAllCities();
    City? GetCityByName(string name);

    void AddCity(City city);
    void DeleteCity(City city);
    void UpdateCity(City city);

    SunriseAndSunset? GetSunriseAndSunset(City city, DateOnly date);
    void AddSunriseAndSunset(SunriseAndSunset sunriseAndSunset);

}