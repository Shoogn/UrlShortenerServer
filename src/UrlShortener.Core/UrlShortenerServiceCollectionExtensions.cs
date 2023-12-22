// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace UrlShortener.Core;

/// <summary>
/// Define the Extensions to register url shortener service.
/// </summary>
public static class UrlShortenerServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TUrlShortener">The url shortener object.</typeparam>
    /// <param name="services">IServiceCollection.</param>
    /// <param name="options">Action<UrlShortenerOptions> this is optional.</param>
    /// <returns>UrlShortenerBuilder object.</returns>
    public static UrlShortenerBuilder AddUrlShortener<TUrlShortener>(this IServiceCollection services, Action<UrlShortenerOptions> options = null)
        where TUrlShortener : class
    {
        services.AddOptions();
        UrlShortenerOptions o = new UrlShortenerOptions();
        services.AddSingleton(o);
        options?.Invoke(o);

        services.TryAddScoped<ILongUrlValidator, LongUrlValidator>();
        services.TryAddScoped<UrlShortenerManager<TUrlShortener>>();

        services.TryAddScoped(static provider =>
        {
            var urlShortenerManager = (IUrlShortenerManager)provider.GetRequiredService(
                typeof(UrlShortenerManager<>).MakeGenericType(typeof(TUrlShortener)));
            return urlShortenerManager;
        });

        return new UrlShortenerBuilder(typeof(TUrlShortener), services);
    }



}

