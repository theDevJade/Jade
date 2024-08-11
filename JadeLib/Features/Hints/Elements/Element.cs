// <copyright file="Element.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RueI.Elements.Enums;
using RueI.Parsing;
using RueI.Parsing.Records;

namespace JadeLib.Features.Hints.Elements;

public abstract class Element
{
    /// <summary>
    /// Gets or sets the options for this element.
    /// </summary>
    public virtual ElementOptions Options { get; protected set; } = ElementOptions.Default;

    /// <summary>
    /// Gets or sets a value indicating whether or not this element is enabled and will show.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the position of the element on a scale from 0-1000, where 0 represents the bottom of the screen and 1000 represents the top.
    /// </summary>
    public float Position { get; set; }

    /// <summary>
    /// Gets or sets the priority of the hint (determining if it shows above another hint).
    /// </summary>
    public int ZIndex { get; set; } = 1;

    /// <summary>
    /// Gets or sets the <see cref="Parser"/> currently in use by this <see cref="Element"/>.
    /// </summary>
    /// <remarks>
    /// Implementations should default this to <see cref="Parser.DefaultParser"/>.
    /// </remarks>
    public Parser Parser { get; set; } = Parser.DefaultParser;

    public abstract string GetText();

    public virtual ParsedData GetParsedData()
    {
        return Parser.DefaultParser.Parse(this.GetText());
    }
}