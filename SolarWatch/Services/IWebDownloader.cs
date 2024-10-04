namespace SolarWatch.Services;

public interface IWebDownloader
{
    public Task<string> GetStringByUrl(string url);
}