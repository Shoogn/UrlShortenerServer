using System.Threading.Tasks;
using System.Threading;

namespace UrlShortener.Core;

public interface IUrlShortenerManager
{
    ValueTask<object> CreateAsync(object urlShortener, string longUrl, CancellationToken cancellationToken = default);

    Task<object> FindByLongUrlAsync(string longUrl, CancellationToken cancellationToken = default);

    ValueTask<string> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken = default);
}
