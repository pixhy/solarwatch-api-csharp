using SolarWatch.Backend.Models;

namespace SolarWatch.Services;

public interface ICityRepository
{
    IEnumerable<City> GetAllCities();
    City? GetCityByName(string name);

    void AddCity(City city);
    void DeleteCity(City city);
    void UpdateCity(City city);
}