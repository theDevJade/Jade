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

        foreach (var value in StatManager.StatPools.SelectMany(keyValuePair => keyValuePair.Value.Stats))
        {
            if (value.Value >= value.LeaderboardThreshold)
            {
                leaderboard.LeaderboardItems.Add(
                    new LeaderboardItem(value.Owner, value.Value, value.LeaderboardMessage, value.LeaderboardPriority));
            }
        }

        leaderboard.LeaderboardItems = leaderboard.LeaderboardItems.OrderBy(e => e.Priority).ToList();

        return leaderboard;
    }
}