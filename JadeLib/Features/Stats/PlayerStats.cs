// <copyright file="PlayerStats.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Exiled.API.Features;
using HarmonyLib;
using JadeLib.Features.Stats.BuiltinStats;

namespace JadeLib.Features.Stats;

/// <summary>
/// A round based player statistics system.
/// </summary>
public static class PlayerStats
{
    /// <summary>
    /// Gets the round-based stat pools.
    /// </summary>
    public static Dictionary<ReferenceHub, StatPool> StatPools { get; } = [];

    internal static List<Stat> stats = [];

    /// <summary>
    /// Gets the available statics.
    /// <remarks>The Owner in each statistic is always null.</remarks>
    /// </summary>
    public static ReadOnlyCollection<Stat> AvailableStats => stats.AsReadOnly();

    /// <summary>
    /// Gets the statistic pool for a reference hub.
    /// </summary>
    /// <param name="hub">The reference hub.</param>
    /// <returns>The statistic pool.</returns>
    public static StatPool GetStatisticsHub(this ReferenceHub hub) => StatPools.GetValueSafe(hub);

    /// <summary>
    /// Gets the statistic pool for a player.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <returns>The statistic pool.</returns>
    public static StatPool GetStatistics(this Player player) => StatPools.GetValueSafe(player.ReferenceHub);
}

/// <summary>
/// A statistic pool for players.
/// </summary>
/// <param name="owner">The owner of this pool.</param>
public sealed class StatPool(ReferenceHub owner)
{
    /// <summary>
    /// The owner of this pool.
    /// </summary>
    public readonly ReferenceHub Owner = owner;

    /// <summary>
    /// The <see cref="KillsStat"/> for this player.
    /// </summary>
    public KillsStat Kills { get; } = new(owner);

    /// <summary>
    /// Gets a list of custom statistics for this pool.
    /// </summary>
    public List<Stat> CustomStats { get; } = [];
}