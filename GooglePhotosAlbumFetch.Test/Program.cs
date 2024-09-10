using System.Text.Json;

namespace GooglePhotosAlbumFetch.Test;

class Program
{
    static void Main(string[] args)
    {
        var images = AlbumFetcher.FetchAlbumImages("https://photos.app.goo.gl/1qNqxN6GohxsW1mk8").Result;

        Console.WriteLine(JsonSerializer.Serialize(images));
    }
}