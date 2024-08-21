#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace JadeLib.Features.Stats.Leaderboard;

public class Leaderboard
{
    private Leaderboard()
    {
    }

    public List<LeaderboardItem> LeaderboardItems { get; private set; } = [];

    public string BuildBroadcast()
    {
        return this.LeaderboardItems
            .Aggregate(
                "<size=20>End Of Round Statistics:</size> ",
                (current, item) => current + $"\n <size=40>{item.Message}</size> ");
    }

    public static Leaderboard BuildLeaderboard()
    {
        var leaderboard = new Leaderboard();
        foreach (var value in from pool in PlayerStats.StatPools
                 from availableStat in PlayerStats.AvailableStats.Where(e => e.IsSubclassOf(typeof(Stat)))
                 let activated = Activator.CreateInstance(availableStat) as Stat
                 select pool.Value.Stats.Get(activated)
                 into obj
                 where !obj.IsNull
                 select obj.Value
                 into value
                 where value.Value >= value.LeaderboardThreshold
                 select value)
        {
            leaderboard.LeaderboardItems.Add(
                new LeaderboardItem(value.Owner, value.Value, value.LeaderboardMessage, value.LeaderboardPriority));
        }

        leaderboard.LeaderboardItems = leaderboard.LeaderboardItems.OrderBy(e => e.Priority).ToList();

        return leaderboard;
    }
}