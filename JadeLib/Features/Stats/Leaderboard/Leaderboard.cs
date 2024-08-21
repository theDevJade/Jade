#region

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

    public string BuildBroadcast(int items = 4)
    {
        return this.LeaderboardItems.Take(items)
            .Aggregate(
                "<size=20>End Of Round Statistics:</size> ",
                (current, item) => current + $"\n <size=25>{item.Message}</size> ");
    }

    public static Leaderboard BuildLeaderboard()
    {
        var leaderboard = new Leaderboard();

        foreach (var value in StatManager.StatPools.SelectMany(keyValuePair => keyValuePair.Value.Stats)
                     .Select(e => e.FindHighestStat(StatManager.GetOfType(e.GetType()))))
        {
            if (value.Value >= value.LeaderboardThreshold &&
                leaderboard.LeaderboardItems.All(e => e.Type != value.GetType())
               )
            {
                leaderboard.LeaderboardItems.Add(
                    new LeaderboardItem(
                        value.Owner,
                        value.Value,
                        value.LeaderboardMessage,
                        value.LeaderboardPriority,
                        value.GetType()));
            }
        }

        leaderboard.LeaderboardItems = leaderboard.LeaderboardItems.OrderBy(e => e.Priority).ToList();

        return leaderboard;
    }
}