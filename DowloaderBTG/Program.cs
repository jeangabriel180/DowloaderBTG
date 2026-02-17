using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;

public class Downloader
{
    private static readonly ConcurrentBag<string> cache = new();
    private static readonly HttpClient client = new();

    public static async Task Main(string[] args)
    {
        var tasks = new Task[10];

        for (int i = 1; i <= 10; i++)
        {
            tasks[i - 1] = DownloadAsync(
                $"https://jsonplaceholder.typicode.com/posts/{i}"
            );
        }

        Console.WriteLine("Downloads started");

        await Task.WhenAll(tasks);

        Console.WriteLine("Downloads finished");
        Console.WriteLine("Cache size: " + cache.Count);
    }

    private static async Task DownloadAsync(string url)
    {
        var data = await client.GetStringAsync(url);
        cache.Add(data);
    }
}
