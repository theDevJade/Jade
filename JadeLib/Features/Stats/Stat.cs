#region

using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using JadeLib.Features.Abstract;

#endregion

namespace JadeLib.Features.Stats;

public abstract class Stat : ModuleSystem<Stat>
{
    public ReferenceHub Owner;

    /// <summary>
    ///     The threshold of this statistic for it to show on the Leaderboard.
    /// </summary>
    public abstract float LeaderboardThreshold { get; set; }

    /// <summary>
    ///     The priority of this statistic for the leaderboard, higher is better.
    /// </summary>
    public abstract int LeaderboardPriority { get; set; }

    /// <summary>
    ///     The message for the leaderboard.
    /// </summary>
    public abstract string LeaderboardMessage { get; }

    /// <summary>
    ///     Gets or sets the value of this <see cref="Stat" />
    /// </summary>
    public virtual float Value { get; protected set; } = 0;

    public void Init(ReferenceHub hub)
    {
        this.Owner = hub;
    }

    protected override void Register()
    {
        StatManager.AvailableStats.Add(this.GetType());
        Log.Info($"{this.GetType().Name} Registering to availablestats");
    }

    /// <summary>
    ///     Runs when registering, used to register events and such for this stat.
    /// </summary>
    public abstract void RegisterStat();

    /// <summary>
    ///     Runs when unregistering, used to unregister events and such for this stat.
    /// </summary>
    public abstract void UnregisterStat();

    /// <summary>
    ///     The handler for the statistic
    /// </summary>
    /// <param name="value">The float./>.</param>
    public abstract void Handle(float value);

    public virtual Stat FindHighestStat(Dictionary<ReferenceHub, Stat> dictionary)
    {
        return dictionary.Aggregate((x, y) => x.Value.Value > y.Value.Value ? x : y).Value;
    }
}