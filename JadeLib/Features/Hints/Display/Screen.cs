﻿// <copyright file="Screen.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#region

using System.Collections.Generic;
using Hints;
using JadeLib.Features.Hints.Elements;
using JadeLib.Features.Hints.Hints;
using JadeLib.Features.Hints.Parsing;
using Utils.NonAllocLINQ;

#endregion

namespace JadeLib.Features.Hints.Display;

public sealed class Screen
{
    public readonly string Identifier;
    public readonly PlayerDisplay OwningDisplay;

    public Screen(PlayerDisplay display, string identifier)
    {
        this.OwningDisplay = display;
        this.Identifier = identifier;
        this.VanillaHint = new VanillaHint(this);
    }

    public List<Element> Elements { get; } = [];

    public VanillaHint VanillaHint { get; }

    public Element AddElement(Element element)
    {
        this.Elements.AddIfNotContains(element);
        return element;
    }

    public bool RemoveElement(Element element)
    {
        return this.Elements.Remove(element);
    }

    /// <summary>
    ///     Forces an update of this <see cref="Screen" />, ignoring the hint ratelimit.
    /// </summary>
    public void ForceUpdate()
    {
        if (!this.Elements.Contains(this.VanillaHint.Element))
        {
            this.Elements.Add(this.VanillaHint.Element);
        }

        var text = ElemCombiner.Combine(this.Elements, this.OwningDisplay.Owner);

        var parameter = new HintParameter[] { new StringHintParameter(text) };
        var effect = new HintEffect[] { HintEffectPresets.TrailingPulseAlpha(1, 1, 1) };
        var hint = new TextHint(text, parameter, effect, float.MaxValue);

        this.OwningDisplay.Owner.connectionToClient.Send(new HintMessage(hint));
    }
}