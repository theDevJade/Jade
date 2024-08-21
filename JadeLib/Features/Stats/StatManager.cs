#region

using System;
using System.Collections.Generic;
using System.Linq;
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
        if (!args.Player.DoNotTrack)
        {
            StatPools.Add(args.Player, new StatPool(args.Player.ReferenceHub));
        }
    }

    private static void OnLeave(LeftEventArgs args)
    {
        StatPools.Remove(args.Player);
    }

    public static StatPool GetStats(this Player player)
    {
        return StatPools[player];
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
        Log.Info("Creating Stat Pool for " + owner.nicknameSync.DisplayName);
        this.Owner = owner;
        foreach (var stat in StatManager.AvailableStats.Select(
                     availableStat => Activator.CreateInstance(availableStat) as Stat))
        {
            Log.Info("Registering stat for " + owner.nicknameSync.DisplayName + " stat " + stat.GetType().Name);
            stat.Init(owner);
            stat?.RegisterStat();
            this.Stats.Add(stat);
        }
    }

    public Stat GetStat<T>()
    {
        return this.Stats.FirstOrDefault(e => e is T);
    }

    public Stat GetStat(Type type)
    {
        return this.Stats.FirstOrDefault(e => type == e.GetType());
    }
}