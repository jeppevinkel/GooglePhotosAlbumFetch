# GooglePhotosAlbumFetch

A package for fetching permanent photo URLs from shared Google Photo albums.

## Usage

```csharp
// Note, the url in this example is not to a valid album.
List<ImageInfo> images = await AlbumFetcher.FetchAlbumImages("https://photos.app.goo.gl/1eGdf543GH4");

foreach(ImageInfo image in images)
{
    Console.WriteLine(image.LargeUrl);
}
```

## Structure

```
{
    Uid: string
    Url: string
    LargeUrl: string
    Width: int
    Height: int
    ImageUpdateDate: long
    AlbumAddDate: long
}
```