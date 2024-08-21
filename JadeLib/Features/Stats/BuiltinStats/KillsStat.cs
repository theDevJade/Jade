// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using JadeLib.Features.Extensions;

#endregion

namespace JadeLib.Features.Stats.BuiltinStats;

/// <inheritdoc />
public sealed class KillsStat : Stat<KillsStat>
{
    public override float LeaderboardThreshold { get; set; } = 7;

    public override int LeaderboardPriority { get; set; } = 1;

    public override string LeaderboardMessage =>
        $"<color=red>{this.Owner.nicknameSync.DisplayName}</color> killed the most people at <color=red>{this.Value.Dplay()}</color> kills";

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