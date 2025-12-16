List<string> pageUrls = new List<string>
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

int MAX_CONCURRENCY = 4;
int MAX_TIMEOUT = 800; // ms

HttpClient client = new HttpClient();
client.Timeout = TimeSpan.FromMilliseconds(MAX_TIMEOUT);

async Task FetchAllPages(List<string> urls)
{
    var semaphore = new SemaphoreSlim(MAX_CONCURRENCY);
    var tasks = new List<Task>();
    var startTime = DateTime.Now;

    foreach (var url in urls)
    {
        tasks.Add(Task.Run(async () =>
        {
            await semaphore.WaitAsync();
            try
            {
                var response = await client.GetAsync(url);
                if (client.Timeout.TotalMilliseconds > MAX_TIMEOUT)
                {
                    Console.WriteLine("TIMEOUT: " + url);
                    return;
                }
                else if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("FAILED: " + url);
                    return;
                }

                Console.WriteLine("SUCCESS: " + url);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
            finally
            {
                semaphore.Release();
                Console.WriteLine("Total application runtime: " + (DateTime.Now - startTime).TotalSeconds + " seconds");
            }
        }));
    }

    await Task.WhenAll(tasks);
}

FetchAllPages(pageUrls).Wait();