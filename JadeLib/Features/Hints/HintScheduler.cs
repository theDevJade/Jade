// <copyright file="HintScheduler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using JadeLib.Features.Hints.Display;
using MEC;

namespace JadeLib.Features.Hints;

public static class HintScheduler
{
    public static CoroutineHandle? Handler { get; private set; } = null;

    internal static void Run()
    {
        if (Handler is { IsRunning: true })
        {
            Timing.KillCoroutines(Handler.Value);
        }

        Handler = Timing.RunCoroutine(Hint());
    }

    private static IEnumerator<float> Hint()
    {
        for (;;)
        {
            foreach (var player in Player.List.Where(e => !e.IsNPC & e.IsConnected))
            {
                PlayerDisplay.Displays[player.ReferenceHub].ForceUpdate();
            }

            yield return Timing.WaitForSeconds(0.527f);
        }
    }
}