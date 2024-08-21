#region

using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using JadeLib.Features.Extensions;
using PlayerRoles;

#endregion

namespace JadeLib.Features.Stats.BuiltinStats;

public class SCPDamageStat : Stat
{
    public override float LeaderboardThreshold { get; set; } = 1000;

    public override int LeaderboardPriority { get; set; } = 2;

    public override string LeaderboardMessage =>
        $"<color=red>{this.Owner.nicknameSync.DisplayName}</color> did <color=red>{this.Value.Dplay()}</color>hp to SCPS";

    public override void RegisterStat()
    {
        Player.Hurt += this.OnHurt;
    }

    public override void UnregisterStat()
    {
        Player.Hurt -= this.OnHurt;
    }

    public override void Handle(float value)
    {
        this.Value += value;
    }

    public void OnHurt(HurtEventArgs args)
    {
        if (args.Attacker != null && args.Player != null && args.Attacker.ReferenceHub == this.Owner &&
            args.Player.Role.Team == Team.SCPs)
        {
            this.Handle(args.Amount);
        }
    }
}