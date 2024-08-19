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
        Exiled.Events.Handlers.Player.Dying += this.OnKill;
    }

    /// <inheritdoc/>
    protected override void UnregisterStat()
    {
        Exiled.Events.Handlers.Player.Dying -= this.OnKill;
    }

    private void OnKill(DyingEventArgs args)
    {
        if (args.Attacker != null && this.Owner != null && args.Attacker.ReferenceHub == this.Owner)
        {
            this.Handle(1);
        }
    }
}