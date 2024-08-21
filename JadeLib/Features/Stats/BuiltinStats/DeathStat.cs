#region

using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using JadeLib.Features.Extensions;

#endregion

namespace JadeLib.Features.Stats.BuiltinStats;

public class DeathStat : Stat
{
    public override float LeaderboardThreshold { get; set; } = 5;

    public override int LeaderboardPriority { get; set; } = 4;

    public override string LeaderboardMessage =>
        $"<color=red>{this.Owner.nicknameSync.DisplayName}</color> died <color=red>{this.Value.Dplay()}</color> times.";

    public override void RegisterStat()
    {
        Player.Died += this.OnUse;
    }

    public override void UnregisterStat()
    {
        Player.Died -= this.OnUse;
    }

    public override void Handle(float value)
    {
        this.Value += value;
    }

    public void OnUse(DiedEventArgs args)
    {
        if (args.Player.ReferenceHub == this.Owner)
        {
            this.Handle(1);
        }
    }
}