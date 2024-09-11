// <copyright file="HintPatch.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#region

using Exiled.API.Features;
using HarmonyLib;
using Hints;
using JadeLib.Features.Hints.Display;
using Hint = Hints.Hint;

#endregion

namespace JadeLib.Features.Hints;

[HarmonyPatch(typeof(HintDisplay), nameof(HintDisplay.Show))]
internal static class HintPatch
{
    [HarmonyPrefix]
    private static bool Prefix(HintDisplay __instance, ref Hint hint)
    {
        if (!Jade.Settings.UseHintSystem)
        {
            return true;
        }

        var plyr = Player.Get(__instance.connectionToClient);
        HintScheduler.EnsureInit(plyr.ReferenceHub);
        if (hint is not TextHint textHint)
        {
            Log.Debug("Not text hint.");
            return true;
        }

        plyr.ReferenceHub.GetDisplay().ActiveScreen.VanillaHint.Add(textHint);

        return false;
    }
}