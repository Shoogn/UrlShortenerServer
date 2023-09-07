// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace UrlShortener.Core;

public class UrlShortenerMiddleware<TUrlShortener> where TUrlShortener : class
{
    private RequestDelegate _nextDelegate;
    private readonly UrlShortenerOptions _urlShortenerOptions;

    public UrlShortenerMiddleware(RequestDelegate nextDelegate,
        UrlShortenerOptions urlShortenerOptions)
    {
        _nextDelegate = nextDelegate;
        _urlShortenerOptions = urlShortenerOptions;
    }

    public async Task Invoke(HttpContext context, UrlShortenerManager<TUrlShortener> urlShortenerManager)
    {
        var path = context.Request.Path.Value;
        if (_urlShortenerOptions.UseUrlShortenerInTheSameDomain)
        {
            if (path.StartsWith(_urlShortenerOptions.ShortUrlPrefix))
            {
                string url = $"{context.Request.Scheme}://{context.Request.Host.Value}{path}";
                var longUrl = await urlShortenerManager.GetLongUrlAsync(url);
                context.Response.Redirect(longUrl, permanent: _urlShortenerOptions.UsePermanentRedirect);
            }
            else
            {
                await _nextDelegate.Invoke(context);
            }
        }
        else
        {
            // so this domain is specified for url shortener only

            var host = context.Request.Host;
            if (_urlShortenerOptions.DomainName.Equals(host.Value))
            {
                var longUrl = await urlShortenerManager.GetLongUrlAsync(path);
                context.Response.Redirect(longUrl, permanent: _urlShortenerOptions.UsePermanentRedirect);
            }
            else
            {
                throw new InvalidOperationException("The domain name is not the same one that is configerd in the UrlShortenerOptions");
            }




        }
    }
}

