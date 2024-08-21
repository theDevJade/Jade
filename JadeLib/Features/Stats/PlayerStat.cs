// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

#endregion

namespace JadeLib.Features.Stats;

/// <summary>
///     A statistic for a player.
/// </summary>
public abstract class Stat<TSelf> : IStat
    where TSelf : Stat<TSelf>
{
    [CanBeNull] public readonly ReferenceHub Owner;

    /// <summary>
    ///     Gets or sets the value of this <see cref="Stat{T}" />
    /// </summary>
    public virtual float Value { get; protected set; } = 0;

    /// <summary>
    ///     The handler for the statistic
    /// </summary>
    /// <param name="value">The float./>.</param>
    public abstract void Handle(float value);

    /// <inheritdoc />
    protected override void Register()
    {
        PlayerStats.stats.Add(MethodBase.GetCurrentMethod()?.DeclaringType);
    }

    /// <summary>
    ///     Runs when registering, used to register events and such for this stat.
    /// </summary>
    protected abstract void RegisterStat();

    /// <summary>
    ///     Runs when unregistering, used to unregister events and such for this stat.
    /// </summary>
    protected abstract void UnregisterStat();

    protected virtual TSelf FindHighestStat(Dictionary<ReferenceHub, TSelf> dictionary)
    {
        return dictionary.Aggregate((x, y) => x.Value.Value > y.Value.Value ? x : y).Value;
    }

    public static TSelf GetHighestStat()
    {
        var self = Activator.CreateInstance<TSelf>();
        var dict = new Dictionary<ReferenceHub, TSelf>();
        foreach (var pair in PlayerStats.StatPools)
        {
            var property = pair.Value.GetCustomStat(self);

            if (!property.IsNull)
            {
                dict.Add(pair.Key, (TSelf)property.Value);
            }
        }

        return self.FindHighestStat(dict);
    }
}