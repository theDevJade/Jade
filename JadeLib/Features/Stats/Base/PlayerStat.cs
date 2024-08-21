// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace JadeLib.Features.Stats;

/// <summary>
///     A statistic for a player.
/// </summary>
public abstract partial class Stat
{
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
    internal abstract void RegisterStat();

    /// <summary>
    ///     Runs when unregistering, used to unregister events and such for this stat.
    /// </summary>
    internal abstract void UnregisterStat();

    protected virtual Stat FindHighestStat(Dictionary<ReferenceHub, Stat> dictionary)
    {
        return dictionary.Aggregate((x, y) => x.Value.Value > y.Value.Value ? x : y).Value;
    }

    public static Stat GetHighestStat<T>()
        where T : Stat
    {
        var self = Activator.CreateInstance<T>();
        var dict = new Dictionary<ReferenceHub, Stat>();
        foreach (var pair in PlayerStats.StatPools)
        {
            var property = pair.Value.GetCustomStat(self);

            if (!property.IsNull)
            {
                dict.Add(pair.Key, property.Value);
            }
        }

        return self.FindHighestStat(dict);
    }
}