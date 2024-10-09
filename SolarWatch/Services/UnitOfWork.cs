namespace SolarWatch.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly SolarWatchApiContext _dbContext;
    public ICityRepository Cities { get; private set; }
    public ISunriseSunsetRepository SunriseSunsets { get; private set; }

    public UnitOfWork(SolarWatchApiContext dbContext)
    {
        _dbContext = dbContext;
        Cities = new CityRepository(_dbContext);
        SunriseSunsets = new SunriseSunsetRepository(_dbContext);
    }

    public int Complete()
    {
        return _dbContext.SaveChanges();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}