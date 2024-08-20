// <copyright file="DisplayEvents.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#region

using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using JadeLib.Features.API.Reflection.Events;

#endregion

namespace JadeLib.Features.Hints.Display;

internal class DisplayEvents
{
    [Listener]
    internal void OnVerify(VerifiedEventArgs args)
    {
        Log.Info($"{args.Player.CustomName} joined, adding playerdisplay.");
        HintScheduler.EnsureInit(args.Player.ReferenceHub);
    }
}