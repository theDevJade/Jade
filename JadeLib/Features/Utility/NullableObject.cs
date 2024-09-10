#region

#nullable enable
using JetBrains.Annotations;

#endregion

namespace JadeLib.Features.UtilityClasses;

/// <summary>
///     A simple nullable object.
/// </summary>
/// <param name="obj">The object to pass, can be null.</param>
/// <typeparam name="T">The type of object.</typeparam>
public sealed class NullableObject<T>([CanBeNull] T? obj)
{
    /// <summary>
    ///     Gets a value indicating whether the nullable object is indeed null.
    /// </summary>
    public bool IsNull => obj == null;

    /// <summary>
    ///     Gets the value, can be null.
    /// </summary>
    public T? Value => obj;
}