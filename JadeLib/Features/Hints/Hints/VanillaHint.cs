// <copyright file="VanillaHint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Hints;
using JadeLib.Features.Hints.Display;
using JadeLib.Features.Hints.Elements;
using JadeLib.Features.Hints.Extensions;
using MEC;

namespace JadeLib.Features.Hints.Hints;

public sealed class VanillaHint
{
    public readonly Screen Owner;

    public string CurrentText { get; private set; } = string.Empty;

    public readonly DynamicElement Element;

    private string Get(HintCtx ctx)
    {
        return this.CurrentText;
    }

    internal VanillaHint(Screen owner)
    {
        this.Owner = owner;
        this.Element = new DynamicElement(400, this.Get);
    }

    public void Add(TextHint hint)
    {
        this.CurrentText = hint.Text;
        Timing.CallDelayed(
            hint.DurationScalar,
            () =>
            {
                if (this.CurrentText == hint.Text)
                {
                    this.CurrentText = string.Empty;
                }
            });
    }
}