// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.EntityFrameworkCore;
using UrlShortener.EntityFramework.Store;
namespace UrlShortener.EntityFramework;

/// <summary>
/// Url Shortener Extensions Container.
/// </summary>
public static class UrlShortenerExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TShortUrl">The type of url shortenerobject.</typeparam>
    /// <param name="modelBuilder">ModelBuilder as extension.</param>
    /// <param name="options">UrlShortenerEntityFrameworkOptions objec.t</param>
    /// <returns><see cref="Microsoft.EntityFrameworkCore.ModelBuilder"/></returns>
    public static ModelBuilder ShortUrlEntitySqlSchemaMapConfiguration<TShortUrl>(this ModelBuilder modelBuilder,
        UrlShortenerEntityFrameworkOptions options)
        where TShortUrl : ShortUrlEntity
    {
        modelBuilder.Entity<TShortUrl>(p =>
        {
            p.ToTable(options.TableName, options.ShemaName);
            p.Property(p => p.Id).ValueGeneratedNever();
            p.Property(p => p.ShortUrl).IsRequired().HasMaxLength(options.ShortUrlMaxLength);
            p.Property(p => p.LongUrl).IsRequired().HasMaxLength(options.LongUrlMaxLength);
        });
        return modelBuilder;
    }
}

