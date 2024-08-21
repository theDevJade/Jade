#region

using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using JadeLib.Features.Extensions;

#endregion

namespace JadeLib.Features.Stats.BuiltinStats;

public class SCPUseStat : Stat
{
    public override float LeaderboardThreshold { get; set; } = 2;

    public override int LeaderboardPriority { get; set; } = 3;

    public override string LeaderboardMessage =>
        $"<color=red>{this.Owner.nicknameSync.DisplayName}</color> consumed <color=red>{this.Value.Dplay()}</color> SCP items";

    public override void RegisterStat()
    {
        Player.UsedItem += this.OnUse;
    }

    public override void UnregisterStat()
    {
        Player.UsedItem -= this.OnUse;
    }

    public override void Handle(float value)
    {
        this.Value += value;
    }

    public void OnUse(UsedItemEventArgs args)
    {
        if (args.Player.ReferenceHub == this.Owner && args.Item.Category == ItemCategory.SCPItem)
        {
            this.Handle(1);
        }
    }
}