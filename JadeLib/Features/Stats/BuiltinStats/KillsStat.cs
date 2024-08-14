// <copyright file="KillsStat.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Exiled.Events.EventArgs.Player;

namespace JadeLib.Features.Stats.BuiltinStats;

/// <inheritdoc />
public sealed class KillsStat(ReferenceHub owner) : NumericalStat<int>(owner)
{
    /// <inheritdoc/>
    public override void Handle(int value)
    {
        this.Value += value;
    }

    /// <inheritdoc/>
    protected override void RegisterStat()
    {
        Exiled.Events.Handlers.Player.KillingPlayer += this.OnKill;
    }

    /// <inheritdoc/>
    protected override void UnregisterStat()
    {
        Exiled.Events.Handlers.Player.KillingPlayer -= this.OnKill;
    }

    private void OnKill(KillingPlayerEventArgs args)
    {
        if (this.Owner != null && args.Player.ReferenceHub == this.Owner)
        {
            this.Handle(1);
        }
    }
}