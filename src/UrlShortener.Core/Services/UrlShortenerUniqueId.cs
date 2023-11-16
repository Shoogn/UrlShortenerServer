// Copyright (c) IshraqSoft. All rights reserved.
// See LICENSE in the project root for license information.

using System;

namespace UrlShortener.Core.Services;
/// <summary>
/// The implementation of this classes is taken from the https://github.com/Shoogn/SnowflakeId repo
/// which is twiteer's unique id algorithm.
/// <see cref="https://github.com/Shoogn/SnowflakeId"/>
/// </summary>
internal class UrlShortenerUniqueId
{
    protected static readonly Lazy<UrlShortenerUniqueId> _lazyInstance = new Lazy<UrlShortenerUniqueId>(() => new UrlShortenerUniqueId());
    public static UrlShortenerUniqueId Instance
    {
        get
        {
            return _lazyInstance.Value;
        }
    }
    // Lock Token
    private readonly object threadLock = new object();

    private long _lastTimestamp = -1L;
    private long _sequence = 0L;

    // result is 22
    const int _timeStampShift = UniqueIdOptionBuilder.TotalBits - UniqueIdOptionBuilder.EpochBits;

    // result is 12
    const int _machaineIdShift = UniqueIdOptionBuilder.TotalBits - UniqueIdOptionBuilder.EpochBits - UniqueIdOptionBuilder.MachineIdBits;

    private readonly long _dataCenterId = 1;

    internal long GenerateSnowflakeId()
    {
        lock (threadLock)
        {
            long currentTimestamp = getTimestamp();

            if (currentTimestamp < _lastTimestamp)
            {
                throw new InvalidOperationException("Error_In_The_Server_Clock");
            }

            if (currentTimestamp == _lastTimestamp)
            {
                // generate a new timestamp when the _sequence is reached the ( 4096 - 1 )

                _sequence = (_sequence + 1) & UniqueIdOptionBuilder.MaxSequenceId;

                if (_sequence == 0)
                {
                    currentTimestamp = waitToGetNextMillis(currentTimestamp);
                }
            }
            else
            {
                _sequence = 0;
            }

            _lastTimestamp = currentTimestamp;

            var result = (currentTimestamp << _timeStampShift) | ((long)_dataCenterId << _machaineIdShift) | (_sequence);

            return result;
        }
    }

    private long getTimestamp()
    {
        return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
    }

    private long waitToGetNextMillis(long currentTimestamp)
    {
        while (currentTimestamp == _lastTimestamp)
        {
            currentTimestamp = getTimestamp();
        }
        return currentTimestamp;
    }

    // Your Epoch Start at 1970 Jan 1s ( Unix Time )
    private static DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}

internal class UniqueIdOptionBuilder
{
    /// <summary>
    /// Total size of the Id in bits, 64
    /// </summary>
    public const int TotalBits = 64;

    /// <summary>
    /// timestamp in bits - 1 bit, 42
    /// </summary>
    public const int EpochBits = 42;

    /// <summary>
    /// machain id in bits, 10
    /// </summary>
    public const int MachineIdBits = 10;

    /// <summary>
    /// sequence in bits, 12
    /// </summary>
    public const int SequenceBits = 12;


    /// <summary>
    /// 99
    /// </summary>
    public static readonly int MaxMachineId = (int)(Math.Pow(2, MachineIdBits) - 1);

    /// <summary>
    /// 143
    /// </summary>
    public static readonly int MaxSequenceId = (int)(Math.Pow(2, SequenceBits) - 1);
}



