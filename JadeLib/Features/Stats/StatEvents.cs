// <copyright file="StatEvents.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Exiled.Events.EventArgs.Player;
using JadeLib.Features.API.Reflection.Events;

namespace JadeLib.Features.Stats;

public sealed class StatEvents
{
    [Listener]
    public void Join(VerifiedEventArgs args)
    {
        PlayerStats.StatPools.Add(args.Player.ReferenceHub, new StatPool(args.Player.ReferenceHub));
    }

    [Listener]
    public void Leave(LeftEventArgs args)
    {
        PlayerStats.StatPools.Remove(args.Player.ReferenceHub);
    }
}