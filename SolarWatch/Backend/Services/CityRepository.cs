using Microsoft.EntityFrameworkCore;
using SolarWatch.Backend.Models;

namespace SolarWatch.Services;

public class CityRepository(SolarWatchApiContext dbContext) : ICityRepository
{
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
    }

    public void DeleteCity(City city)
    {
        dbContext.Remove(city);
    }

    public void UpdateCity(City city)
    {  
        dbContext.Update(city);
    }
}