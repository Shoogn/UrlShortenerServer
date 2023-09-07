// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

namespace UrlShortener.Core;

/// <summary>
/// Configure the UrlShortener options .
/// </summary>
public class UrlShortenerOptions
{
    /// <summary>
    /// Get or sets the short url prefix, and this is used if the <see cref="UseUrlShortenerInTheSameDomain"/> is equal to true.
    /// The max length value of this property is <code><3/code> letter and the default value is <code>/sh</code>.
    /// </summary>
    public string ShortUrlPrefix { get; set; } = "/sh";

    /// <summary>
    /// Determine if you would like to Use Url Shortener In The Same Domain, by default is true.
    /// </summary>
    public bool UseUrlShortenerInTheSameDomain { get; set; } = true;

    /// <summary>
    /// Set the Domain Name value if the <see cref="UseUrlShortenerInTheSameDomain"/> is equal to false.
    /// </summary>
    public string DomainName { get; set; }

    /// <summary>
    /// Set the Redirection type, and by default is Permanent.
    /// </summary>
    public bool UsePermanentRedirect { get; set; } = true;
}

