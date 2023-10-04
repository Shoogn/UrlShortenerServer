// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using UrlShortener.Core;
using UrlShortener.EntityFramework.Store;

namespace UrlShortener.EntityFramework;

/// <summary>
/// The default url shortener store.
/// </summary>
public class UrlShortenerStore : UrlShortenerStore<ShortUrlEntity>

{
    public UrlShortenerStore(DbContext context) : base(context)
    {
    }
}

/// <summary>
/// Url shortener store that take a type argument, this type should be (is) ShortUrlEntity object.
/// </summary>
/// <typeparam name="TUrlShortener">The type of the url shortener object, the default one is <see cref="ShortUrlEntity"/>.</typeparam>
public class UrlShortenerStore<TUrlShortener> : UrlShortenerStore<TUrlShortener, DbContext>
    where TUrlShortener : ShortUrlEntity, new()
{
    public UrlShortenerStore(DbContext context) : base(context)
    {
    }
}

/// <summary>
/// The main url shortener store.
/// </summary>
/// <typeparam name="TUrlShortener">The type of the url shortener objec, this type should be (is) ShortUrlEntity object. </typeparam>
/// <typeparam name="TContext">Microsoft.EntityFrameworkCore.DbContext.</typeparam>
public class UrlShortenerStore<TUrlShortener, TContext>
    : IUrlShortenerStore<TUrlShortener>
    where TUrlShortener : ShortUrlEntity, new()
    where TContext : DbContext
{
    public UrlShortenerStore(TContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        Context = context;
    }
    public virtual TContext Context { get; private set; }
    private DbSet<TUrlShortener> UrlShortenersContext { get { return Context.Set<TUrlShortener>(); } }

    public virtual async ValueTask<TUrlShortener> CreateAsync(TUrlShortener urlShortener, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var urlShortenerobj = UrlShortenersContext.Add(urlShortener).Entity;
        int savedItem = await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return savedItem > 0 ? urlShortenerobj : throw new InvalidOperationException("longUrl is not saved");
    }

    public virtual Task<TUrlShortener> FindByLongUrlAsync(string longUrl, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (string.IsNullOrWhiteSpace(longUrl))
            throw new ArgumentNullException(nameof(longUrl));

        return UrlShortenersContext.FirstOrDefaultAsync(x => x.LongUrl == longUrl, cancellationToken);
    }

    public virtual async ValueTask<string> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (string.IsNullOrWhiteSpace(shortUrl))
            throw new ArgumentNullException(nameof(shortUrl));

        var data = await UrlShortenersContext.FirstOrDefaultAsync(x => x.ShortUrl == shortUrl, cancellationToken);
        if (data is null)
            throw new NullReferenceException(nameof(shortUrl));
        return data.LongUrl;
    }


    public virtual Task SetUrlShortenerAsync(TUrlShortener urlShortener, long id, string longUrl, string shortUrl, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        urlShortener.Id = id;
        urlShortener.LongUrl = longUrl;
        urlShortener.ShortUrl = shortUrl;
        return Task.CompletedTask;
    }
}

