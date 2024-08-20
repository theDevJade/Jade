// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#nullable enable

#region

using System;

#endregion

namespace JadeLib.Features.UtilityClasses;

/// <summary>
///     A dynamic and versatile system that allows you to add translations to enums.
/// </summary>
/// <typeparam name="TSelf">A reference to itself.</typeparam>
/// <typeparam name="TEnum">The type of the enum you are providing translations for.</typeparam>
/// <typeparam name="TReturn">The return value for retrieving translations.</typeparam>
/// <typeparam name="TAdditionalPasser">An additional passer in EnumTranslation::Get.</typeparam>
public abstract class EnumTranslation<TSelf, TEnum, TReturn, TAdditionalPasser>
    where TEnum : Enum
    where TSelf : EnumTranslation<TSelf, TEnum, TReturn, TAdditionalPasser>
    where TAdditionalPasser : class?
{
    /// <summary>
    ///     Retrieves the translation for this <see cref="EnumTranslation{TSelf,TEnum,TReturn,TAdditionalPasser}" />
    /// </summary>
    /// <param name="enum">The Enum.</param>
    /// <param name="passer">The additional passer, defaults to null.</param>
    /// <returns>
    ///     The <typeparamref name="TReturn" /> of this
    ///     <see cref="EnumTranslation{TSelf,TEnum,TReturn,TAdditionalPasser}" />.
    /// </returns>
    public static TReturn Get(TEnum @enum, TAdditionalPasser? passer = null)
    {
        var self = Activator.CreateInstance<TSelf>();
        return self.GetNonStatic(ref @enum, passer);
    }

    protected abstract TReturn GetNonStatic(ref TEnum @enum, TAdditionalPasser? passer = null);
}