namespace SolarWatch.Services;

public interface IUnitOfWork : IDisposable
{
    ICityRepository Cities { get; }
    ISunriseSunsetRepository SunriseSunsets { get; }
    public void SaveChanges();

}