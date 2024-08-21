#region

using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using JadeLib.Features.Extensions;
using PlayerRoles;

#endregion

namespace JadeLib.Features.Stats.BuiltinStats;

public sealed class SCPDamageStat : Stat
{
    public override float LeaderboardThreshold { get; set; } = 2000;

    public override int LeaderboardPriority { get; set; } = 1;

    public override string LeaderboardMessage =>
        $"<color=red>{this.Owner.nicknameSync.DisplayName}</color> did the most damage at <color=red>{this.Value.Dplay()}</color>hp";

    internal override void RegisterStat()
    {
        Player.Hurt += this.OnDamage;
    }

    internal override void UnregisterStat()
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