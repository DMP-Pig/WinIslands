using System.IO;

using System.Net.Http;

using System.Security.Cryptography;

using System.Text;

namespace WinIslands.Services;

/// <summary>
/// Downloads / extracts album art to a local cache so the UI can bind a file path
/// instead of keeping streams alive, and so Cider artwork is fetched only once.
/// </summary>
public static class ArtworkCache
{
    private static readonly HttpClient Http = CreateClient();

    private static HttpClient CreateClient()
    {
        var handler = new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.All };
        var client = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(8) };
        client.DefaultRequestHeaders.UserAgent.ParseAdd("WinIslands/0.1");
        return client;
    }

    public static string CacheKey(params string[] parts)
    {
        var joined = string.Join("\u0001", parts.Where(p => !string.IsNullOrEmpty(p)));
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(joined));
        return Convert.ToHexString(bytes)[..24];
    }

    /// <summary>Save raw image bytes to the cache and return the file path.</summary>
    public static string SaveBytes(byte[] data, string key)
    {
        try
        {
            var ext = SniffExtension(data);
            var path = Path.Combine(AppPaths.ThumbCacheDir, $"{key}{ext}");
            File.WriteAllBytes(path, data);
            return path;
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"ArtworkCache.SaveBytes failed: {ex.Message}");
            return string.Empty;
        }
    }

    /// <summary>Download a remote image (Cider artwork). Returns local path or "" on failure.</summary>
    public static async Task<string> DownloadAsync(string url, string key, CancellationToken ct = default)
    {
        try
        {
            var uri = new Uri(url);
            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) return string.Empty;

            using var resp = await Http.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
            if (!resp.IsSuccessStatusCode) return string.Empty;
            var data = await resp.Content.ReadAsByteArrayAsync(ct).ConfigureAwait(false);
            return SaveBytes(data, key);
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Artwork download failed for {url}: {ex.Message}");
            return string.Empty;
        }
    }

    /// <summary>Build an artwork path from bytes sniffed from a stream (SMTC thumbnails).</summary>
    public static async Task<string> SaveStreamAsync(Func<Stream, Task> writeTo, string key)
    {
        try
        {
            using var ms = new MemoryStream();
            await writeTo(ms).ConfigureAwait(false);
            return SaveBytes(ms.ToArray(), key);
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Artwork stream save failed: {ex.Message}");
            return string.Empty;
        }
    }

    private static string SniffExtension(byte[] data)
    {
        if (data.Length > 3 && data[0] == 0xFF && data[1] == 0xD8) return ".jpg";
        if (data.Length > 8 &&
            data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4E && data[3] == 0x47) return ".png";
        if (data.Length > 3 && data[0] == 'G' && data[1] == 'I' && data[2] == 'F') return ".gif";
        if (data.Length > 2 && data[0] == 'B' && data[1] == 'M') return ".bmp";
        return ".jpg";
    }
}

