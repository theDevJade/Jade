#region

using System;

#endregion

namespace JadeLib.Features.Positioning;

/// <summary>
///     Represents a time class that holds a timestamp (in long) and supports various operations.
/// </summary>
public class Time
{
    /// <summary>
    ///     Ticks per second (10 million ticks in 1 second).
    /// </summary>
    private const long TicksPerSecond = TimeSpan.TicksPerSecond;

    /// <summary>
    ///     Ticks per minute.
    /// </summary>
    private const long TicksPerMinute = TimeSpan.TicksPerMinute;

    /// <summary>
    ///     Ticks per hour.
    /// </summary>
    private const long TicksPerHour = TimeSpan.TicksPerHour;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Time" /> class with the current time.
    /// </summary>
    public Time()
    {
        this.CurrentTime = DateTime.UtcNow.Ticks;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Time" /> class with a specific time (in ticks).
    /// </summary>
    /// <param name="ticks">The time in ticks (long).</param>
    public Time(long ticks)
    {
        this.CurrentTime = ticks;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Time" /> class with a specific DateTime.
    /// </summary>
    /// <param name="dateTime">The specific <see cref="DateTime" /> instance.</param>
    public Time(DateTime dateTime)
    {
        this.CurrentTime = dateTime.Ticks;
    }

    /// <summary>
    ///     Gets the current time as a long representing ticks.
    /// </summary>
    public long CurrentTime { get; }

    #region Conversion Methods

    /// <summary>
    ///     Converts the current time to seconds.
    /// </summary>
    /// <returns>The time in seconds as a double.</returns>
    public double ToSeconds()
    {
        return (double)this.CurrentTime / TicksPerSecond;
    }

    /// <summary>
    ///     Converts the current time to minutes.
    /// </summary>
    /// <returns>The time in minutes as a double.</returns>
    public double ToMinutes()
    {
        return (double)this.CurrentTime / TicksPerMinute;
    }

    /// <summary>
    ///     Converts the current time to hours.
    /// </summary>
    /// <returns>The time in hours as a double.</returns>
    public double ToHours()
    {
        return (double)this.CurrentTime / TicksPerHour;
    }

    #endregion

    #region Operator Overloads

    public static Time operator +(Time a, Time b)
    {
        return new Time(a.CurrentTime + b.CurrentTime);
    }

    public static Time operator -(Time a, Time b)
    {
        return new Time(a.CurrentTime - b.CurrentTime);
    }

    public static Time operator *(Time a, long scalar)
    {
        return new Time(a.CurrentTime * scalar);
    }

    public static Time operator /(Time a, long scalar)
    {
        return new Time(a.CurrentTime / scalar);
    }

    public static bool operator ==(Time a, Time b)
    {
        return a.CurrentTime == b.CurrentTime;
    }

    public static bool operator !=(Time a, Time b)
    {
        return a.CurrentTime != b.CurrentTime;
    }

    public static bool operator >(Time a, Time b)
    {
        return a.CurrentTime > b.CurrentTime;
    }

    public static bool operator <(Time a, Time b)
    {
        return a.CurrentTime < b.CurrentTime;
    }

    public static bool operator >=(Time a, Time b)
    {
        return a.CurrentTime >= b.CurrentTime;
    }

    public static bool operator <=(Time a, Time b)
    {
        return a.CurrentTime <= b.CurrentTime;
    }

    public override bool Equals(object obj)
    {
        if (obj is Time time)
        {
            return this == time;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return this.CurrentTime.GetHashCode();
    }

    #endregion
}