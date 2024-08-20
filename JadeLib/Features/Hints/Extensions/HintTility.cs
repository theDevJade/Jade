﻿// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

namespace JadeLib.Features.Hints.Extensions;

/// <summary>
///     Provides helpful functions for dealing with elements and hints.
/// </summary>
/// <remarks>
///     <see cref="HintTility" /> is a helper class that provides methods for handling scaled and functional values,
///     along with other helpful methods that may not work exclusively with hints.
/// </remarks>
public static class HintTility
{
    /// <summary>
    ///     Cleans a string by wrapping it in noparses, and removes any noparse closer tags existing in it already.
    /// </summary>
    /// <param name="text">The string to clean.</param>
    /// <returns>The cleaned string.</returns>
    public static string GetCleanText(string text)
    {
        var cleanText = text.Replace("</noparse>", "</nopa​rse>"); // zero width space
        return $"<noparse>{cleanText}</noparse>";
    }

    /// <summary>
    ///     Converts a scaled position from 0-1000 into functional pixels (offset from baseline).
    /// </summary>
    /// <param name="position">The position to convert.</param>
    /// <returns>The converted value.</returns>
    public static float ScaledPositionToFunctional(float position)
    {
        return (position * -2.14f) + 755f;
    }

    /// <summary>
    ///     Converts a functional position into a scaled position.
    /// </summary>
    /// <param name="position">The position to convert.</param>
    /// <returns>The converted value.</returns>
    public static float FunctionalToScaledPosition(float position)
    {
        return (position - 755f) / 2.14f;
    }
}