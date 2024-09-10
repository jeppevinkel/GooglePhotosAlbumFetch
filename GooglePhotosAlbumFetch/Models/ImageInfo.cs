namespace GooglePhotosAlbumFetch.Models;

public class ImageInfo
{
    public string Uid { get; init; } = string.Empty;
    /// <summary>
    /// The default URL returns a small version of the image.
    /// </summary>
    public string Url { get; init; } = string.Empty;
    /// <summary>
    /// A modified URL that returns the full image.
    /// </summary>
    public string LargeUrl => $"{Url}=w{Width}-h{Height}";
    public int Width { get; init; }
    public int Height { get; init; }

    /// <summary>
    /// Time the image was updated in Unix Epoch time.
    /// </summary>
    public long ImageUpdateDate { get; init; }

    /// <summary>
    /// Time the image was added to the album in Unix Epoch time.
    /// </summary>
    public long AlbumAddDate { get; init; }
}