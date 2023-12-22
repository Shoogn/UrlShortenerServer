// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using System;

namespace UrlShortener.Core;

/// <summary>
/// Url shortener builder object.
/// </summary>
public sealed class UrlShortenerBuilder
{
    /// <summary>
    /// Default constructor that accept two parameters.
    /// </summary>
    /// <param name="shortUrlType">shortUrlType.</param>
    /// <param name="services">IServiceCollection.</param>
    /// <exception cref="ArgumentException">ArgumentException.</exception>
    /// <exception cref="ArgumentNullException">ArgumentNullException.</exception>
    public UrlShortenerBuilder(Type shortUrlType, IServiceCollection services)
    {
        if (shortUrlType.IsValueType)
            throw new ArgumentException("short url object can not be a value type", nameof(ShortUrlType));
        ShortUrlType = shortUrlType;
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// The Iservice collection.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Type of the short url object.
    /// </summary>
    public Type ShortUrlType { get; set; }

    public UrlShortenerBuilder AddLongUrlValidator<TValidator>() where TValidator : class
    {
        Services.AddScoped(typeof(ILongUrlValidator), typeof(TValidator));
        return this;
    }

    public UrlShortenerBuilder Configure(Action<UrlShortenerOptions> configuration)
    {
        if(configuration == null)
        {
            throw new ArgumentNullException(nameof (configuration));
        }

        Services.Configure(configuration);
        return this;
    }
}

