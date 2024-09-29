namespace SolarWatch.Services;

public interface IWebDownloader
{
    public string GetStringByUrl(string url);
}