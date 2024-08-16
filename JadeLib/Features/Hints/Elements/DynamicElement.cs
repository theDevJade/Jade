// <copyright file="DynamicElement.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace JadeLib.Features.Hints.Elements;

public sealed class DynamicElement(int position, Element.HintContent content) : Element
{
    public override float Position { get; set; } = position;

    public override string GetText(HintCtx ctx)
    {
        return content.Invoke(ctx);
    }
}