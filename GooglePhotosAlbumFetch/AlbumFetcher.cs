using System.Text.Json;
using System.Text.RegularExpressions;
using GooglePhotosAlbumFetch.Models;

namespace GooglePhotosAlbumFetch;

public static class AlbumFetcher
{
    private static async Task<string> GetAlbumHtml(string albumSharedUrl,
        CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();

        return await client.GetStringAsync(albumSharedUrl, cancellationToken);
    }

    private static string HtmlToJson(string input)
    {
        var regex = new Regex(@"(?<=AF_initDataCallback\()(?=.*data)(\{[\s\S]*?)(\);<\/script>)",
            RegexOptions.Compiled);
        MatchCollection matches = regex.Matches(input);

        var match = matches.Select(m => m.Groups[1].Value).Aggregate("", (a, b) => a.Length > b.Length ? a : b);

        var dataRegex = new Regex(@"data:(\[.*\])", RegexOptions.Compiled);

        return dataRegex.Match(match).Groups[1].Value;
    }

    private static JsonElement? DeserializeJson(string input)
    {
        try
        {
            return JsonSerializer.Deserialize<JsonElement>(input);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return null;
        }
    }

    private static List<ImageInfo>? GetImageInfos(JsonElement data)
    {
        var result = new List<ImageInfo>();

        if (data.ValueKind != JsonValueKind.Array || data.GetArrayLength() < 1)
        {
            return null;
        }

        foreach (JsonElement element in data[1].EnumerateArray())
        {
            if (element.ValueKind != JsonValueKind.Array || element.GetArrayLength() < 6)
            {
                continue;
            }

            var uid = element[0].GetString();
            var imageUpdateDate = element[2].GetInt64();
            var albumAddDate = element[5].GetInt64();

            if (string.IsNullOrWhiteSpace(uid) || imageUpdateDate == 0 || albumAddDate == 0)
                continue;

            JsonElement detail = element[1];
            if (detail.ValueKind != JsonValueKind.Array || detail.GetArrayLength() < 3)
            {
                continue;
            }

            var url = detail[0].GetString();
            var width = detail[1].GetInt32();
            var height = detail[2].GetInt32();

            if (string.IsNullOrWhiteSpace(url) || width == 0 || height == 0)
                continue;

            result.Add(new ImageInfo
            {
                Uid = uid,
                Url = url,
                Width = width,
                Height = height,
                ImageUpdateDate = imageUpdateDate,
                AlbumAddDate = albumAddDate
            });
        }

        return result;
    }

    public static async Task<List<ImageInfo>> FetchAlbumImages(string albumUrl, CancellationToken cancellationToken = default)
    {
        var html = await GetAlbumHtml(albumUrl, cancellationToken);
        var json = HtmlToJson(html);

        var data = DeserializeJson(json);

        if (data is null)
        {
            throw new InvalidDataException("The response from the URL is not valid.");
        }

        return GetImageInfos(data.Value) ?? [];
    }
}