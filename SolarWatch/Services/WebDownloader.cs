using System.Net;

namespace SolarWatch.Services;

public class WebDownloader : IWebDownloader
{
    public string GetStringByUrl(string url)
    {
        using var client = new WebClient();

        return client.DownloadString(url);
    }
}