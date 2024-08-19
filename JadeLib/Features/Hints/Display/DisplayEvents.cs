// <copyright file="DisplayEvents.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using JadeLib.Features.API.Reflection.Events;

namespace JadeLib.Features.Hints.Display;

internal class DisplayEvents
{
    [Listener]
    internal void OnVerify(VerifiedEventArgs args)
    {
        Log.Info($"{args.Player.CustomName} joined, adding playerdisplay.");
        PlayerDisplay.AddDisplay(args.Player.ReferenceHub, new PlayerDisplay(args.Player.ReferenceHub));
    }

    [Listener]
    internal void OnDisconnect(LeftEventArgs args)
    {
        Log.Info($"{args.Player.CustomName} left the server");
        PlayerDisplay.RemoveDisplay(args.Player.ReferenceHub);
    }
}