// <copyright file="ConditionalHint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Exiled.API.Features;
using HintServiceMeow.Core.Models.Hints;

namespace JadeLib.Features.Hints.Hints;

/// <summary>
/// A custom hint that takes in a condition to display.
/// </summary>
public abstract class ConditionalHint : GlobalHint
{
    /// <inheritdoc/>
    public abstract override string UniqueIdentifier { get; set; }

    /// <inheritdoc/>
    public override bool ShouldTick { get; set; } = false;

    public abstract string Content(AbstractHint.TextUpdateArg arg);

    /// <summary>
    /// A function returning if the hint should be enabled.
    /// </summary>
    /// <param name="player">The player the condition should check with.</param>
    /// <returns>A value indicating whether the hint should be enabled.</returns>
    protected abstract bool Condition(Player player);

    public override string GetContent(AbstractHint.TextUpdateArg context)
    {
        var player = Player.Get(context.Player);
        return this.Condition(player) switch
        {
            true => this.Content(context),
            false => string.Empty
        };
    }
}