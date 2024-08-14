﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exiled.API.Features;
using JadeLib.Features.Hints.Parsing.Tags;
using NorthwoodLib.Pools;

namespace JadeLib.Features.Hints.Parsing;

/// <summary>
/// Builds <see cref="Parser"/>s.
/// </summary>
public sealed class ParserBuilder
{
    private readonly List<RichTextTag> currentTags = ListPool<RichTextTag>.Shared.Rent(10);
    private readonly List<Parser> backups = ListPool<Parser>.Shared.Rent(2);

    /// <summary>
    /// Initializes a new instance of the <see cref="ParserBuilder"/> class.
    /// </summary>
    public ParserBuilder()
    {
    }

    /// <summary>
    /// Gets the number of tags within this <see cref="ParserBuilder"/>.
    /// </summary>
    public int TagsCount => this.currentTags.Count;

    /// <summary>
    /// Adds new <see cref="RichTextTag"/>s from an assembly by getting all of the <see cref="RichTextTagAttribute"/> classes.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/> to get the classes from.</param>
    /// <returns>A reference to this <see cref="ParserBuilder"/>.</returns>
    public ParserBuilder AddFromAssembly(Assembly assembly)
    {
        MethodInfo addTag = typeof(ParserBuilder).GetMethod(nameof(this.AddTag));

        foreach (Type type in assembly.GetTypes())
        {
            if (type.GetCustomAttributes(typeof(RichTextTagAttribute), true).Any())
            {
                if (type.IsSubclassOf(typeof(RichTextTag)))
                {
                    MethodInfo generic = addTag.MakeGenericMethod(type);
                    generic.Invoke(this, Array.Empty<object>());
                }
                else
                {
                    Log.Error("Hint Developer Issue");
                }
            }
        }

        return this;
    }

    // instead of using a RichTextTag instance a generic type is used to
    // ensure that only one canonical instance of a RichTextTag type is in a parser
    // duplicates of a type may exist, but not different instances

    /// <summary>
    /// Gets the <see cref="SharedTag{T}"/> of a <see cref="RichTextTag"/> type and adds it to the builder.
    /// </summary>
    /// <typeparam name="T">The type of the tag to create.</typeparam>
    /// <returns>A reference to this <see cref="ParserBuilder"/>.</returns>
    public ParserBuilder AddTag<T>()
        where T : RichTextTag, new()
    {
        T tag = SharedTag<T>.Singleton;
        this.currentTags.Add(tag);

        return this;
    }

    /// <summary>
    /// Imports all of the <see cref="RichTextTag"/>s from a <see cref="Parser"/>, adding it to the builder.
    /// </summary>
    /// <param name="parser">The <see cref="Parser"/> to import the tags from.</param>
    /// <returns>A reference to this <see cref="ParserBuilder"/>.</returns>
    public ParserBuilder ImportFrom(Parser parser)
    {
        this.backups.Add(parser);
        return this;
    }

    /// <summary>
    /// Builds this <see cref="ParserBuilder"/> into a <see cref="Parser"/>.
    /// </summary>
    /// <returns>The built <see cref="Parser"/>.</returns>
    public Parser Build() => new(this.currentTags, this.backups);

    /// <summary>
    /// Adds all of the tags from an <see cref="IEnumerable{RichTextTag}"/>.
    /// </summary>
    /// <param name="tags">The tags to add.</param>
    internal void AddTags(IEnumerable<RichTextTag> tags)
    {
        foreach (RichTextTag tag in tags)
        {
            this.currentTags.Add(tag);
        }
    }
}
