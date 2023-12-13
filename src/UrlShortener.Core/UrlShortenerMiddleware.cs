// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace UrlShortener.Core;

public class UrlShortenerMiddleware<TUrlShortener> where TUrlShortener : class
{
    private RequestDelegate _nextDelegate;

    public UrlShortenerMiddleware(RequestDelegate nextDelegate)
    {
        _nextDelegate = nextDelegate;
    }

    public async Task Invoke(HttpContext context, 
        UrlShortenerManager<TUrlShortener> urlShortenerManager, 
        IOptions<UrlShortenerOptions> urlShortenerOptions)
    {
        var path = context.Request.Path.Value;
        if (urlShortenerOptions.Value.UseUrlShortenerInTheSameDomain)
        {
            if (path.StartsWith(urlShortenerOptions.Value.ShortUrlPrefix))
            {
                string url = $"{context.Request.Scheme}://{context.Request.Host.Value}{path}";
                var longUrl = await urlShortenerManager.GetLongUrlAsync(url);
                context.Response.Redirect(longUrl, permanent: urlShortenerOptions.Value.UrlRedirectionType == RedirectionType.Permanent ? true : false);
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
            if (urlShortenerOptions.Value.DomainName.Equals(host.Value))
            {
                var longUrl = await urlShortenerManager.GetLongUrlAsync(path);
                context.Response.Redirect(longUrl, permanent: urlShortenerOptions.Value.UrlRedirectionType == RedirectionType.Permanent ? true : false);
            }
            else
            {
                throw new InvalidOperationException("The domain name is not the same one that is configerd in the UrlShortenerOptions");
            }




        }
    }
}

