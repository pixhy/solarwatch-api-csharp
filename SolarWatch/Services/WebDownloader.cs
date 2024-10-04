using System.Net;

namespace SolarWatch.Services;

public class WebDownloader : IWebDownloader
{
    public async Task<string> GetStringByUrl(string url)
    {
        using var client = new HttpClient();

        var response = await client.GetAsync(url);

        return await response.Content.ReadAsStringAsync();
    }
}