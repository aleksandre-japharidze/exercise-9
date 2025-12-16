using System.Reflection.Metadata;

public class Program
{
    private List<string> pageUrls = new List<string>
    {
        "https://ipinfo.io/161.185.160.93/geo",
        "https://official-joke-api.appspot.com/random_joke",
        "https://randomuser.me/api/",
        "https://api.zippopotam.us/us/33162",
        "https://api.agify.io?name=meelad",
        "https://api.zippopotam.us/us/33162",
        "https://api.nationalize.io?name=nathaniel",
        "https://api.genderize.io?name=luc"
    };

    private const int MAX_CONCURRENCY = 4;

    private HttpClient httpClient = new HttpClient();

    private async Task FetchAllPagesAsync()
    {
        foreach (var pageUrl in pageUrls)
        {
            var response = await httpClient.GetAsync(pageUrl);
            var statusCode = response.StatusCode;
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Status Code from {pageUrl}: {statusCode}");
            Console.WriteLine($"Response from {pageUrl}: {content}\n");
        }
    }

    public static void Main(string[] args)
    {
        var program = new Program();
        program.FetchAllPagesAsync().GetAwaiter().GetResult();
    }
}