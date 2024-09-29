namespace SolarWatch.Services;

public interface IGeocodingService
{
    public Coordinate GetCoordinatesByCity(string city);
}