using System.Threading.Tasks;
using System.Threading;

namespace UrlShortener.Core;

/// <summary>
/// An interface holds the required methods that integrate with the 
/// url shortener object that is define in the program.cs file when adding AddUrlShortener service.
/// </summary>
public interface IUrlShortenerManager
{
    /// <summary>
    /// Create a new shor url.
    /// </summary>
    /// <param name="urlShortenerObj"></param>
    /// <param name="longUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Return the url object.</returns>
    ValueTask<object> CreateAsync(object urlShortenerObj, string longUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the url object by long url.
    /// </summary>
    /// <param name="longUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Return the url object.</returns>
    Task<object> FindByLongUrlAsync(string longUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the long url by the short url.
    /// </summary>
    /// <param name="shortUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Return long url.</returns>
    ValueTask<string> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken = default);
}
