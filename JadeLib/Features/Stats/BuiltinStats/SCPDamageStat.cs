#region

using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using PlayerRoles;

#endregion

namespace JadeLib.Features.Stats.BuiltinStats;

public sealed class SCPDamageStat(ReferenceHub owner) : Stat<SCPDamageStat>(owner)
{
    public SCPDamageStat() : this(null)
    {
    }

    protected override void RegisterStat()
    {
        Player.Hurt += this.OnDamage;
    }

    protected override void UnregisterStat()
    {
        Player.Hurt -= this.OnDamage;
    }

    public override void Handle(float value)
    {
        this.Value += value;
    }

    private void OnDamage(HurtEventArgs args)
    {
        if (args.Player == null || args.Attacker == null || this.Owner == null ||
            args.Attacker.ReferenceHub != this.Owner)
        {
            return;
        }

        if (args.Player.Role.Team == Team.SCPs)
        {
            this.Handle((int)args.Amount);
        }
    }
}