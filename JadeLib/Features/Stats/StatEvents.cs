// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using Exiled.Events.EventArgs.Player;
using JadeLib.Features.API.Reflection.Events;

#endregion

namespace JadeLib.Features.Stats;

public sealed class StatEvents
{
    [Listener]
    public void Join(VerifiedEventArgs args)
    {
        if (!args.Player.DoNotTrack)
        {
            PlayerStats.StatPools.Add(args.Player.ReferenceHub, new StatPool(args.Player.ReferenceHub));
        }
    }

    [Listener]
    public void Leave(LeftEventArgs args)
    {
        PlayerStats.StatPools.Remove(args.Player.ReferenceHub);
    }
}