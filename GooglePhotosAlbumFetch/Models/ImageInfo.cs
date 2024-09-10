namespace GooglePhotosAlbumFetch.Models;

public class ImageInfo
{
    public string Uid { get; set; } = string.Empty;
    public List<string> Url { get; set; } = [];
    public int Width { get; set; }
    public int Height { get; set; }
    public long ImageUpdateDate { get; set; }
    public long AlbumAddDate { get; set; }
}