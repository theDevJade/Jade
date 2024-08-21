// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Exiled.API.Features;
using HarmonyLib;
using JadeLib.Features.UtilityClasses;

#endregion

namespace JadeLib.Features.Stats;

/// <summary>
///     A round based player statistics system.
/// </summary>
public static class PlayerStats
{
    internal static List<Type> stats = [];

    /// <summary>
    ///     Gets the round-based stat pools.
    /// </summary>
    public static Dictionary<ReferenceHub, StatPool> StatPools { get; } = [];

    /// <summary>
    ///     Gets the available statics.
    ///     <remarks>The Owner in each statistic is always null.</remarks>
    /// </summary>
    public static ReadOnlyCollection<Type> AvailableStats => stats.AsReadOnly();

    /// <summary>
    ///     Gets the statistic pool for a reference hub.
    /// </summary>
    /// <param name="hub">The reference hub.</param>
    /// <returns>The statistic pool.</returns>
    public static StatPool GetStatisticsHub(this ReferenceHub hub)
    {
        return StatPools.GetValueSafe(hub);
    }

    /// <summary>
    ///     Gets the statistic pool for a player.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <returns>The statistic pool.</returns>
    public static StatPool GetStatistics(this Player player)
    {
        return StatPools.GetValueSafe(player.ReferenceHub);
    }
}

/// <summary>
///     A statistic pool for players.
/// </summary>
/// <param name="owner">The owner of this pool.</param>
public sealed class StatPool
{
    /// <summary>
    ///     The owner of this pool.
    /// </summary>
    public readonly ReferenceHub Owner;

    public StatPool(ReferenceHub owner)
    {
        this.Owner = owner;
        foreach (var availableStat in PlayerStats.AvailableStats)
        {
            var stat = Activator.CreateInstance(availableStat) as IStat;
            this.Stats.Add(stat);
        }
    }

    /// <summary>
    ///     Gets a list of custom statistics for this pool.
    /// </summary>
    public CustomList<IStat> Stats { get; } = [];

    /// <summary>
    ///     Get a custom statistic based on a type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <typeparam name="T">The typeparam (Defaulted to type).</typeparam>
    /// <returns>A <see cref="NullableObject{T}" /> (possibly) containing the custom stat.</returns>
    public NullableObject<Stat<T>> GetCustomStat<T>(T type)
        where T : Stat<T>
    {
        return new NullableObject<Stat<T>>((Stat<T>)this.Stats.Get(type).Value);
    }
}