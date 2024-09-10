#region

using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using JadeLib.Features.Extensions;
using Player = Exiled.Events.Handlers.Player;

#endregion

namespace JadeLib.Features.Stats.BuiltinStats;

/// <inheritdoc />
public sealed class KillsStat : Stat
{
    public override float LeaderboardThreshold { get; set; } = 3;

    public override int LeaderboardPriority { get; set; } = 1;

    public override string LeaderboardMessage =>
        $"<color=red>{this.Owner.nicknameSync.DisplayName}</color> killed <color=red>{this.Value.Dplay()}</color> people";

    /// <inheritdoc />
    public override void Handle(float value)
    {
        this.Value += value;
        Log.Info($"Adding value {this.Owner.nicknameSync.DisplayName} value at {this.Value}");
    }

    /// <inheritdoc />
    public override void RegisterStat()
    {
        Player.Dying += this.OnKill;
    }

    /// <inheritdoc />
    public override void UnregisterStat()
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