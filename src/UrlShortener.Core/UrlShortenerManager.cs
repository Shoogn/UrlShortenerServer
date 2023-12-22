// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UrlShortener.Core.Services;

namespace UrlShortener.Core;

/// <summary>
/// The container class that provide methods to manage all things about url shortener.
/// </summary>
/// <typeparam name="TUrlShortener">the type of the url shortener.</typeparam>
public class UrlShortenerManager<TUrlShortener> : IUrlShortenerManager where TUrlShortener : class
{
    private const string _alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    public UrlShortenerManager(IUrlShortenerStore<TUrlShortener> urlShortenerStore,
        IOptions<UrlShortenerOptions> options, IEnumerable<ILongUrlValidator> longUrlValidators)
    {
        if (urlShortenerStore == null)
            throw new ArgumentNullException(nameof(urlShortenerStore));
        UrlShortenerStore = urlShortenerStore;

        UrlShortenerOptions = options?.Value ?? new UrlShortenerOptions();

        if (longUrlValidators is not null)
        {
            foreach (var validator in longUrlValidators)
                LongUrlValidators.Add(validator);
        }


    }
    protected IUrlShortenerStore<TUrlShortener> UrlShortenerStore { get; private set; }
    public UrlShortenerOptions UrlShortenerOptions { get; set; }
    public IList<ILongUrlValidator> LongUrlValidators { get; } = new List<ILongUrlValidator>();



    /// <summary>
    /// Create a new short url.
    /// </summary>
    /// <param name="urlShortener">TUrlShortener object</param>
    /// <param name="longUrl">longUrl.</param>
    /// <param name="cancellationToken">cancellationToken.</param>
    /// <returns></returns>
    public virtual async ValueTask<TUrlShortener> CreateAsync(TUrlShortener urlShortener, string longUrl, CancellationToken cancellationToken = default)
    {
        var longUrlTemp = await ValidateLongUrlAsync(longUrl);
        var isLongUrlExist = await FindByLongUrlAsync(longUrl, cancellationToken);
        if (isLongUrlExist is not null)
            return isLongUrlExist;

        long id = UrlShortenerUniqueId.Instance.GenerateSnowflakeId();
        string shorturlAsBase62 = ToBase62(id);
        string shorturl = string.Empty;
        if (UrlShortenerOptions.UseUrlShortenerInTheSameDomain)
        {
            string host = longUrlTemp.Host;
            shorturl = $"{longUrlTemp.Scheme}://{host}:{longUrlTemp.Port}{UrlShortenerOptions.ShortUrlPrefix}/{shorturlAsBase62}";
        }
        else
        {
            shorturl = $"{UrlShortenerOptions.DomainName}/{shorturlAsBase62}";
        }

        await UrlShortenerStore.SetUrlShortenerAsync(urlShortener, id, longUrl, shorturl, cancellationToken).ConfigureAwait(false);
        return await CreateAsync(urlShortener).ConfigureAwait(false);
    }

    /// <summary>
    /// Get the url shortener object from back store by providing the long url.
    /// </summary>
    /// <param name="longUrl">The long url.</param>
    /// <param name="cancellationToken">cancellationToken.</param>
    /// <returns></returns>
    public virtual async Task<TUrlShortener> FindByLongUrlAsync(string longUrl, CancellationToken cancellationToken = default)
    {
        return await UrlShortenerStore.FindByLongUrlAsync(longUrl, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Get the long url object from back store by providing the short url.
    /// </summary>
    /// <param name="shortUrl">The short url.</param>
    /// <param name="cancellationToken">cancellationToken.</param>
    /// <returns></returns>
    public async ValueTask<string> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken = default)
    {
        var data = await UrlShortenerStore.GetLongUrlAsync(shortUrl, cancellationToken);
        return data;
    }

    protected async Task<Uri> ValidateLongUrlAsync(string longUrl)
    {
        foreach (var url in LongUrlValidators)
        {
            await url.ValidateAsync(longUrl).ConfigureAwait(false);
        }

        return new Uri(longUrl);


    }

    /// <inheritdoc/>
    async ValueTask<object> IUrlShortenerManager.CreateAsync(object urlShortenerObj, string longUrl, CancellationToken cancellationToken)
    {
        return await CreateAsync((TUrlShortener)urlShortenerObj, longUrl, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    async Task<object> IUrlShortenerManager.FindByLongUrlAsync(string longUrl, CancellationToken cancellationToken)
    {
        return await FindByLongUrlAsync(longUrl, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    async ValueTask<string> IUrlShortenerManager.GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken)
    {
        return await GetLongUrlAsync(shortUrl, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Create a new short url.
    /// </summary>
    /// <param name="longUrl">The long url.</param>
    /// <param name="cancellationToken">cancellationToken.</param>
    /// <returns></returns>
    private async ValueTask<TUrlShortener> CreateAsync(TUrlShortener urlShortener, CancellationToken cancellationToken = default)
    {
        var rseult = await UrlShortenerStore.CreateAsync(urlShortener, cancellationToken).ConfigureAwait(false);
        return rseult;
    }

    private string ToBase62(long number)
    {
        long n = number;
        long basis = 62;
        string ret = string.Empty;
        while (n > 0)
        {
            long temp = n % basis;
            ret = _alphabet[(int)temp] + ret;
            n /= basis;
        }
        return ret;
    }
}

