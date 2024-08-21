// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq;
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
    ///     Initializes a new instance of the <see cref="Stat{TSelf}" /> class.
    /// </summary>
    public Stat()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Stat{TSelf}" /> class.
    /// </summary>
    /// <param name="owner">The owner of this statistic.</param>
    protected Stat(ReferenceHub owner)
    {
        this.Owner = owner;
        this.RegisterStat();
    }

    /// <summary>
    ///     Gets or sets the value of this <see cref="NumericalStat{T}" />
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
        PlayerStats.stats.Add(this);
        this.RegisterStat();
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
            object property;
            try
            {
                property = typeof(StatPool).GetProperties()
                    .First(e => e.CanRead & (e.PropertyType == typeof(TSelf)))
                    .GetValue(pair.Value);
            }
            catch
            {
                // ignored
                property = pair.Value.GetCustomStat(self).Value;
            }

            dict.Add(pair.Key, (TSelf)property);
        }

        return self.FindHighestStat(dict);
    }
}