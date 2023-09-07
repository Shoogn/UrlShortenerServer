// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace UrlShortener.Core;

/// <summary>
/// Define the Extensions to register the url shortener application builder.
/// </summary>
public static class UrlShortenerRedirectionBuilderExtensions
{
    /// <summary>
    /// Add the url shortener redirection app builder.
    /// </summary>
    /// <typeparam name="TUrlShortener">url shortener object</typeparam>
    /// <param name="app">IApplicationBuilder.</param>
    /// <returns>IApplicationBuilder.</returns>
    /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
    public static IApplicationBuilder UseUrlShortenerRedirection<TUrlShortener>(this IApplicationBuilder app)
        where TUrlShortener : class
    {
        if(app == null)
            throw new ArgumentNullException(nameof(app));

        var shortenerOptions = app.ApplicationServices.GetService<UrlShortenerOptions>();
        if(shortenerOptions ==null) 
            throw new ArgumentNullException(nameof(shortenerOptions));

        app.UseMiddleware<UrlShortenerMiddleware<TUrlShortener>>(shortenerOptions);

        return app;
    }
}

