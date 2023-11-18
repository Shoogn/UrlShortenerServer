// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;

namespace UrlShortener.Core;

/// <summary>
/// Define a virtual method to validate the long url.
/// </summary>
public class LongUrlValidator : ILongUrlValidator
{
    /// <summary>
    /// Validate teh long url as an asynchronous operation.
    /// </summary>
    /// <param name="longUrl">Long url as string.</param>
    /// <returns>A boolean value determin if the long url is a valid url or not.</returns>
    public virtual ValueTask<bool> ValidateAsync(string longUrl)
    {
        try
        {
            Uri longUrlTemp = new Uri(longUrl);
            return ValueTask.FromResult(true);
        }
        catch (ArgumentNullException)
        {
            throw new ArgumentNullException(nameof(longUrl));
        }
        catch (UriFormatException)
        {
            throw new UriFormatException(nameof(longUrl));
        }
    }
}
