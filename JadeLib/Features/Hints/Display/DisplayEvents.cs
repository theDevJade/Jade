// <copyright file="DisplayEvents.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Exiled.Events.EventArgs.Player;
using JadeLib.Features.API.Reflection.Events;

namespace JadeLib.Features.Hints.Display;

internal class DisplayEvents
{
    [Listener]
    internal void OnVerify(VerifiedEventArgs args)
    {
        var playerDisplay = new PlayerDisplay(args.Player.ReferenceHub);
    }

    [Listener]
    internal void OnDisconnect(LeftEventArgs args)
    {
        PlayerDisplay.RemoveDisplay(args.Player.ReferenceHub);
    }
}