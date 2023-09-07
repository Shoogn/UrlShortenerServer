// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using UrlShortener.Core;

namespace UrlShortener.EntityFramework;

/// <summary>
/// Define the Extensions to register the url shortener services.
/// </summary>
public static class UrlShortenerEntityFrameworkBuilderExtensions
{
    /// <summary>
    /// Add url shortener entity framework stores to the service container.
    /// </summary>
    /// <typeparam name="TContext">The tye of the dbcontext object.</typeparam>
    /// <param name="builder">UrlShortenerBuilder.</param>
    /// <param name="storeOptionsAction">Action<UrlShortenerEntityFrameworkOptions>, this is optional.</param>
    /// <returns>UrlShortenerBuilder object<./returns>
    /// <exception cref="InvalidOperationException">InvalidOperationException.</exception>
    public static UrlShortenerBuilder AddUrlShortenerEntityFrameworkStores<TContext>(this UrlShortenerBuilder builder,
       Action<UrlShortenerEntityFrameworkOptions> storeOptionsAction = null)
        where TContext : DbContext
    {
        var storeOptions = new UrlShortenerEntityFrameworkOptions();
        builder.Services.AddSingleton(storeOptions);
        storeOptionsAction?.Invoke(storeOptions);

        var shotUrlType = builder.ShortUrlType;
        if (builder.ShortUrlType == null)
        {
            throw new InvalidOperationException(nameof(builder.ShortUrlType));

        }
        var contextType = typeof(TContext);
        Type urlShortenerStoreType;
        var urlShortenerDbContext = FindGenericBaseType(contextType, typeof(UrlShortenerDbContext<>));
        if (urlShortenerDbContext == null)
        {
            urlShortenerStoreType = typeof(UrlShortenerStore<>).MakeGenericType(shotUrlType);
        }
        else
        {
            urlShortenerStoreType = typeof(UrlShortenerStore<,>).MakeGenericType(shotUrlType, contextType);
        }
        builder.Services.TryAddScoped(typeof(IUrlShortenerStore<>).MakeGenericType(shotUrlType), urlShortenerStoreType);
        return builder;
    }


    private static Type FindGenericBaseType(Type currentType, Type genericBaseType)
    {
        Type type = currentType;
        while (type != null)
        {
            var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
            if (genericType != null && genericType == genericBaseType)
            {
                return type;
            }
            type = type.BaseType;
        }
        return null;
    }
}




