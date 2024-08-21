// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;

#endregion

namespace JadeLib.Features.Stats.BuiltinStats;

/// <inheritdoc />
public sealed class KillsStat(ReferenceHub owner) : Stat<KillsStat>(owner)
{
    /// <inheritdoc />
    public override void Handle(float value)
    {
        this.Value += value;
    }

    /// <inheritdoc />
    protected override void RegisterStat()
    {
        Player.Dying += this.OnKill;
    }

    /// <inheritdoc />
    protected override void UnregisterStat()
    {
        Player.Dying -= this.OnKill;
    }

    private void OnKill(DyingEventArgs args)
    {
        if (args.Attacker != null && this.Owner != null && args.Attacker.ReferenceHub == this.Owner)
        {
            this.Handle(1);
        }
    }
}