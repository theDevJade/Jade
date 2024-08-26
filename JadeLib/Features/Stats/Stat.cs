#region

using System.Collections.Generic;
using System.Linq;
using JadeLib.Features.Abstract;
using Utils.NonAllocLINQ;

#endregion

namespace JadeLib.Features.Stats;

/// <summary>
///     The base class for a statistic.
/// </summary>
public abstract class Stat : ModuleSystem<Stat>
{
    /// <summary>
    ///     Gets the owner of this <see cref="Stat" />
    /// </summary>
    public ReferenceHub Owner { get; private set; }

    /// <summary>
    ///     Gets or sets the threshold for this statistic to show on the Leaderboard.
    /// </summary>
    public abstract float LeaderboardThreshold { get; set; }

    /// <summary>
    ///     Gets or sets the priority to show this statistic on the leaderboard, 1 being first.
    /// </summary>
    public abstract int LeaderboardPriority { get; set; }

    /// <summary>
    ///     Gets the message for the leaderboard.
    /// </summary>
    public abstract string LeaderboardMessage { get; }

    /// <summary>
    ///     Gets or sets the value of this <see cref="Stat" />
    /// </summary>
    public virtual float Value { get; protected set; } = 0;

    /// <summary>
    ///     Initializes this statistic with a <see cref="ReferenceHub" />
    /// </summary>
    /// <param name="hub">The ReferenceHub to assign.</param>
    public void Init(ReferenceHub hub)
    {
        this.Owner = hub;
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
    /// <param name="value">The float to handle./>.</param>
    public abstract void Handle(float value);

    /// <summary>
    ///     Finds the highest statistic in the <paramref name="dictionary" />
    /// </summary>
    /// <param name="dictionary">The dictionary to search.</param>
    /// <returns>The highest stat.</returns>
    public virtual Stat FindHighestStat(Dictionary<ReferenceHub, Stat> dictionary)
    {
        return dictionary.Aggregate((x, y) => x.Value.Value > y.Value.Value ? x : y).Value;
    }

    /// <inheritdoc />
    protected override void Register()
    {
        StatManager.AvailableStats.AddIfNotContains(this.GetType());
    }
}