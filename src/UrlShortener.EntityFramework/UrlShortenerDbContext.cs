// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using UrlShortener.EntityFramework.Store;

namespace UrlShortener.EntityFramework;

/// <summary>
/// The dbcontect object to interact with the database provider,
/// this one the the <see cref="ShortUrlEntity"/> as default object.
/// </summary>
public class UrlShortenerDbContext : UrlShortenerDbContext<ShortUrlEntity>
{
    public UrlShortenerDbContext(DbContextOptions options) : base(options) { }
    protected UrlShortenerDbContext() { }
}

/// <summary>
/// The dbcontect object to interact with the database provider.
/// </summary>
/// <typeparam name="TShortUrl">The type of the url shortener.</typeparam>
public abstract class UrlShortenerDbContext<TShortUrl> : DbContext
    where TShortUrl : ShortUrlEntity
{
    public UrlShortenerDbContext(DbContextOptions options) : base(options) { }
    protected UrlShortenerDbContext() { }
    public virtual DbSet<TShortUrl> ShortUrls { get; set; }

    public UrlShortenerEntityFrameworkOptions ShortenerConfigurationOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (ShortenerConfigurationOptions is null)
        {
            ShortenerConfigurationOptions = this.GetService<UrlShortenerEntityFrameworkOptions>();
            if (ShortenerConfigurationOptions is null)
            {
                throw new ArgumentNullException(nameof(ShortenerConfigurationOptions),
                    "Make sure to register UrlShortenerEntityFrameworkOptions in the DI service container");
            }
        }
        modelBuilder.ShortUrlEntitySqlSchemaMapConfiguration<TShortUrl>(ShortenerConfigurationOptions);
        base.OnModelCreating(modelBuilder);
    }
}

