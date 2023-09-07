// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

namespace UrlShortener.EntityFramework.Store;

/// <summary>
/// The short url object.
/// </summary>
public class ShortUrlEntity
{
    /// <summary>
    /// Initilize constructor.
    /// </summary>
    public ShortUrlEntity() { }

    /// <summary>
    /// Unique Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Get or set the long url.
    /// </summary>
    public string LongUrl { get; set; } = default!;

    /// <summary>
    /// Get or set the short url.
    /// </summary>
    public string ShortUrl { get; set; } = default!;
}

