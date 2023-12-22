// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

namespace UrlShortener.Core;
public enum RedirectionType : byte
{
    /// <summary>
    /// Permanent (HTTP 301).
    /// </summary>
    Permanent = 0,

    /// <summary>
    /// Temporary (HTTP 302).
    /// </summary>
    Temporary = 1,
}
