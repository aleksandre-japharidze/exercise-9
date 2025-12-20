List<string> urls = new List<string>
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

var semaphore = new SemaphoreSlim(4);

async Task FetchApi(string url, int maxTimeout = 800)
{
    HttpClient client = new HttpClient();
    client.Timeout = TimeSpan.FromMilliseconds(maxTimeout);

    await semaphore.WaitAsync();
    Console.WriteLine("FETCHING: " + url);

    try
    {
        var startTime = DateTime.Now;
        var response = await client.GetAsync(url);
        var endTime = DateTime.Now;
        if (client.Timeout.TotalMilliseconds > maxTimeout)
        {
            Console.WriteLine("TIMEOUT: " + url + " -- TIME: " + (endTime - startTime).TotalMilliseconds);
            return;
        }
        else if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine("FAILED: " + url + " -- TIME: " + (endTime - startTime).TotalMilliseconds);
            return;
        }
        Console.WriteLine("SUCCESS: " + url + " -- TIME: " + (endTime - startTime).TotalMilliseconds);
    }
    catch (Exception e)
    {
        Console.WriteLine("ERROR: " + e.Message);
    }
    finally
    {
        semaphore.Release();
        Console.WriteLine("DONE: " + url);
    }
}

Task.WaitAll(urls.Select(url => FetchApi(url)).ToArray());
Console.WriteLine("ALL DONE");