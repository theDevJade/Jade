// <copyright file="DisplayEvents.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#region

using Exiled.Events.EventArgs.Player;
using JadeLib.Features.Abstract.FeatureGroups.Events;

#endregion

namespace JadeLib.Features.Hints.Display;

internal class DisplayEvents
{
    [Listener]
    internal void OnVerify(VerifiedEventArgs args)
    {
        HintScheduler.EnsureInit(args.Player.ReferenceHub);
    }
}