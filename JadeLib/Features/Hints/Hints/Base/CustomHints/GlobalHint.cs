// <copyright file="GlobalHint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Exiled.Events.EventArgs.Player;
using JadeLib.Features.API.Reflection.Events;

namespace JadeLib.Features.Hints.Hints.Base.CustomHints;

/// <inheritdoc />
public abstract class GlobalHint : CustomHint
{
    /// <inheritdoc/>
    public abstract override string UniqueIdentifier { get; set; }

    /// <inheritdoc/>
    public override bool ShouldTick { get; set; } = false;

    /// <inheritdoc/>
    public override void Tick()
    {
    }

    [Listener]
    public void OnVerify(VerifiedEventArgs args)
    {
        this.AddToPlayer(args.Player);
    }
}