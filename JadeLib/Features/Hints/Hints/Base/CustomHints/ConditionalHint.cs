// <copyright file="ConditionalHint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Exiled.API.Features;
using JadeLib.Features.Hints.Elements;

namespace JadeLib.Features.Hints.Hints.Base.CustomHints;

/// <summary>
/// A custom hint that takes in a condition to display.
/// </summary>
public abstract class ConditionalHint : GlobalHint
{
    /// <inheritdoc/>
    public abstract override int Position { get; set; }

    /// <inheritdoc/>
    public abstract override string UniqueIdentifier { get; set; }

    /// <inheritdoc/>
    public override bool ShouldTick { get; set; } = false;

    protected abstract string Content(HintCtx context);

    /// <summary>
    /// A function returning if the hint should be enabled.
    /// </summary>
    /// <param name="player">The player the condition should check with.</param>
    /// <returns>A value indicating whether the hint should be enabled.</returns>
    protected abstract bool Condition(Player player);

    public override string GetContent(HintCtx context)
    {
        var player = Player.Get(context.Player);
        return this.Condition(player) switch
        {
            true => this.Content(context),
            false => string.Empty
        };
    }
}