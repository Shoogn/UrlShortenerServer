// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using System.ComponentModel;

namespace UrlShortener.EntityFramework;

/// <summary>
/// Configure the UrlShortener entity framework options.
/// </summary>
public sealed class UrlShortenerEntityFrameworkOptions
{
    /// <summary>
    /// The default the length for short and long url.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public const int DEFAULT_MAX_LENGTH = 450;

    /// <summary>
    /// Set or get the max length constraint for short url.
    /// </summary>
    public int ShortUrlMaxLength { get; set; } = DEFAULT_MAX_LENGTH;

    /// <summary>
    /// Set or get the max length constraint for long url.
    /// </summary>
    public int LongUrlMaxLength { get; set; } = DEFAULT_MAX_LENGTH;

    /// <summary>
    /// Set or Get the default schema name for the short url table.
    /// </summary>
    public string ShemaName { get; set; } = "dbo";

    /// <summary>
    /// Get or set the table name.
    /// </summary>
    public string TableName { get; set; } = "ShortUrls";
}

