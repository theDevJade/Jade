#region

using JadeLib.Features.Hints.Parsing.Enums;
using NorthwoodLib.Pools;

#endregion

namespace JadeLib.Features.Hints.Parsing.Records;

/// <summary>
///     Defines a record that contains information about measurement info.
/// </summary>
/// <param name="value">The value of the measurement.</param>
/// <param name="style">The style of the measurement.</param>
/// <remarks>
///     This provides a convenient way to specify both the value and unit for a measurement,
///     as the base value when converted to pixels can differ depending on the
///     context of the measurement.
/// </remarks>
public record struct MeasurementInfo(float value, MeasurementUnit style)
{
    /// <summary>
    ///     Attempts to extract a <see cref="MeasurementInfo" /> from a string.
    /// </summary>
    /// <param name="content">The content to parse.</param>
    /// <param name="info">The returned info, if true.</param>
    /// <returns>true if the string was valid, otherwise false.</returns>
    public static bool TryParse(string content, out MeasurementInfo info)
    {
        var paramBuffer = StringBuilderPool.Shared.Rent(25);
        var style = MeasurementUnit.Pixels;

        var hasPeriod = false;

        foreach (var ch in content)
        {
            if (ch == 'e')
            {
                style = MeasurementUnit.Ems;
                break;
            }

            if (ch == '%')
            {
                style = MeasurementUnit.Percentage;
                break;
            }

            if (ch == 'p') // pixels
            {
                break;
            }

            if (ch == '.')
            {
                if (!hasPeriod)
                {
                    hasPeriod = true;
                    paramBuffer.Append('.');
                }
            }
            else if (char.IsDigit(ch))
            {
                paramBuffer.Append(ch);
            }
        }

        var bufferString = StringBuilderPool.Shared.ToStringReturn(paramBuffer);
        if (float.TryParse(bufferString, out var result) && result < Constants.MEASUREMENTVALUELIMIT)
        {
            info = new MeasurementInfo(result, style);
            return true;
        }

        info = default;
        return false;
    }

    /// <summary>
    ///     Gets a string representation of this <see cref="MeasurementInfo" />.
    /// </summary>
    /// <returns>A new value.</returns>
    public readonly override string ToString()
    {
        return this.value + this.style switch
        {
            MeasurementUnit.Ems => "e",
            MeasurementUnit.Percentage => "%",
            _ => string.Empty,
        };
    }
}