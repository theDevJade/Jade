﻿// <copyright file="HintScheduler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using JadeLib.Features.Hints.Display;
using JadeLib.Features.Hints.Elements;
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

        Handler = Timing.RunCoroutine(Hint(), Segment.Update);
    }

    internal static void RunIfNot()
    {
        if (Handler is { IsRunning: true })
        {
            return;
        }

        Handler = Timing.RunCoroutine(Hint(), Segment.Update);
    }

    private static IEnumerator<float> Hint()
    {
        for (;;)
        {
            foreach (var keyValuePair in PlayerDisplay.Displays)
            {
                Log.Info(
                    $"Debug ): Updating PlayerDisplay {keyValuePair.Key.nicknameSync.DisplayName}. {string.Join("\n", keyValuePair.Value.ActiveScreen.Elements.Select(e => e.GetText(new HintCtx(e, keyValuePair.Key))))}");
                keyValuePair.Value.ForceUpdate();
            }

            yield return Timing.WaitForSeconds(0.527f);
        }
    }
}