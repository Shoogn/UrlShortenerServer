// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using System.Threading.Tasks;
using System.Threading;

namespace UrlShortener.Core;

/// <summary>
/// The Url Shortener Store interface.
/// </summary>
/// <typeparam name="TUrlShortener"></typeparam>
public interface IUrlShortenerStore<TUrlShortener> where TUrlShortener : class
{
    /// <summary>
    /// Create a new short url.
    /// </summary>
    /// <param name="id">Unique Id.</param>
    /// <param name="longUrl">Long url.</param>
    /// <param name="shortUrl">Short url.</param>
    /// <param name="cancellationToken">cancellationToken.</param>
    /// <returns>Type of Url Shortener object.</returns>
    ValueTask<TUrlShortener> CreateAsync(long id, string longUrl, string shortUrl, CancellationToken cancellationToken);

    /// <summary>
    /// Get the short url object by long url.
    /// </summary>
    /// <param name="longUrl">longUrl.</param>
    /// <param name="cancellationToken">cancellationToken.</param>
    /// <returns>Type of Url Shortener object.</returns>
    Task<TUrlShortener> FindByLongUrlAsync(string longUrl, CancellationToken cancellationToken);

    /// <summary>
    /// Get the long url by short url.
    /// </summary>
    /// <param name="shortUrl">Short url.</param>
    /// <param name="cancellationToken">cancellationToken.</param>
    /// <returns>The long url as string.</returns>
    ValueTask<string> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken);
}

