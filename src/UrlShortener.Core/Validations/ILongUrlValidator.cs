// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using System.Threading.Tasks;

namespace UrlShortener.Core;

/// <summary>
/// Provides an abstraction to validate the long url.
/// </summary>
public interface ILongUrlValidator
{
    /// <summary>
    /// Validate teh long url as an asynchronous operation.
    /// </summary>
    /// <param name="longUrl">Long url as string.</param>
    /// <returns>A boolean value determin if the long url is a valid url or not.</returns>
    ValueTask<bool> ValidateAsync(string longUrl);
}
