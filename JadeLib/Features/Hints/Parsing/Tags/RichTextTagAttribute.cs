﻿// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;

#endregion

namespace JadeLib.Features.Hints.Parsing.Tags;

/// <summary>
///     Defines a <see cref="RichTextTag" /> for RueI.
/// </summary>
/// <remarks>
///     You can apply this <see cref="Attribute" /> to classes that inherit from <see cref="RichTextTag" /> to define a
///     custom <see cref="RichTextTag" /> easily.
///     This attribute is used exclusive by the <see cref="ParserBuilder.AddFromAssembly(System.Reflection.Assembly)" />
///     method.
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public class RichTextTagAttribute : Attribute
{
}