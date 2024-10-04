namespace SolarWatch.Services;

public interface IGeocodingService
{
    public Task<Coordinate> GetCoordinatesByCity(string city);
}