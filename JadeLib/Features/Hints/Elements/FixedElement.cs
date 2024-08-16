// <copyright file="FixedElement.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace JadeLib.Features.Hints.Elements;

public sealed class FixedElement(string text, int position) : Element
{
    private string text = text;

    public override float Position { get; set; } = position;

    public override string GetText(HintCtx ctx)
    {
        return this.text;
    }
}