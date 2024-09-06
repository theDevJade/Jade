#region

using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using JetBrains.Annotations;

#endregion

namespace JadeLib.Features.Stats;

public static class StatManager
{
    /// <summary>
    ///     A Dictionary containing <see cref="StatPool" />s for each player.
    ///     <remarks>WILL BE NULL if the player has DNT enabled</remarks>
    /// </summary>
    public static Dictionary<Player, StatPool> StatPools = [];

    public static List<Type> AvailableStats { get; } = [];

    internal static void Register()
    {
        Exiled.Events.Handlers.Player.Verified += OnJoin;
        Exiled.Events.Handlers.Player.Left += OnLeave;
    }

    private static void OnJoin(VerifiedEventArgs args)
    {
        if (!args.Player.DoNotTrack)
        {
            StatPools.Add(args.Player, new StatPool(args.Player.ReferenceHub));
        }
    }

    private static void OnLeave(LeftEventArgs args)
    {
        StatPools.Remove(args.Player);
    }

    /// <summary>
    ///     Gets the StatPool for a player.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <returns>The StatPool, or null if the player has DNT.</returns>
    [CanBeNull]
    public static StatPool GetStats(this Player player)
    {
        return StatPools.ContainsKey(player) ? null : StatPools[player];
    }

    public static Dictionary<ReferenceHub, Stat> GetOfType(Type type)
    {
        var dict = new Dictionary<ReferenceHub, Stat>();
        foreach (var keyValuePair in StatPools)
        {
            var stat = keyValuePair.Value.GetStat(type);
            dict.Add(keyValuePair.Key.ReferenceHub, stat);
        }

        return dict;
    }
}

public class StatPool
{
    public readonly ReferenceHub Owner;

    public List<Stat> Stats = [];

    public StatPool(ReferenceHub owner)
    {
        this.Owner = owner;
        foreach (var stat in StatManager.AvailableStats.Select(
                     availableStat => Activator.CreateInstance(availableStat) as Stat))
        {
            stat?.Init(owner);
            stat?.RegisterStat();
            this.Stats.Add(stat);
        }
    }

    /// <summary>
    ///     Retrieves a stat for this statpool.
    /// </summary>
    /// <typeparam name="T">The type of stat.</typeparam>
    /// <returns>The stat.</returns>
    public Stat GetStat<T>()
    {
        return this.Stats.FirstOrDefault(e => e is T);
    }

    /// <summary>
    ///     Retrieves a stat for this statpool.
    /// </summary>
    /// <param name="type">The type of stat.</param>
    /// <returns>The stat.</returns>
    public Stat GetStat(Type type)
    {
        return this.Stats.FirstOrDefault(e => type == e.GetType());
    }
}