#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

#endregion

namespace JadeLib.Features.Stats;

public static class StatManager
{
    public static Dictionary<Player, StatPool> StatPools = [];

    public static List<Type> AvailableStats { get; } = [];

    internal static void Register()
    {
        Exiled.Events.Handlers.Player.Verified += OnJoin;
        Exiled.Events.Handlers.Player.Left += OnLeave;
    }

    private static void OnJoin(VerifiedEventArgs args)
    {
        StatPools.Add(args.Player, new StatPool(args.Player.ReferenceHub));
    }

    private static void OnLeave(LeftEventArgs args)
    {
        StatPools.Remove(args.Player);
    }

    public static StatPool GetStats(this Player player)
    {
        return StatPools[player];
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
                     availableStat => Activator.CreateInstance(
                         availableStat,
                         BindingFlags.Public,
                         null,
                         [owner],
                         CultureInfo.CurrentCulture) as Stat))
        {
            stat?.RegisterStat();
            this.Stats.Add(stat);
        }
    }

    public Stat GetStat<T>()
    {
        return this.Stats.FirstOrDefault(e => e is T);
    }

    public Stat GetStat<T>(Type type)
    {
        return this.Stats.FirstOrDefault(e => e is T);
    }
}