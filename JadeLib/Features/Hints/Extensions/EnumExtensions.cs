#region

using JadeLib.Features.Hints.Enums;

#endregion

namespace JadeLib.Features.Hints.Extensions;

/// <summary>
///     Provides extensions for working with RueI <see cref="Enum" />s.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    ///     Quickly determines if an <see cref="ElementOptions" /> has another <see cref="ElementOptions" />.
    /// </summary>
    /// <param name="first">The first <see cref="ElementOptions" />.</param>
    /// <param name="second">The other <see cref="ElementOptions" />.</param>
    /// <returns>A value indicating whether or not the first has all of the flags of the second.</returns>
    /// <inheritdoc cref="HasFlagFast(Roles, Roles)" path="/remarks" />
    public static bool HasFlagFast(this ElementOptions first, ElementOptions second)
    {
        return (first & second) == second;
    }
}