using System;
using HarmonyLib;
using Hints;
using PluginAPI.Core;
using Hint = Hints.Hint;

namespace HintServiceMeow.Core.Utilities
{
    [HarmonyPatch(typeof(HintDisplay), nameof(HintDisplay.Show))]
    internal static class HintDisplayPatch
    {
        static bool Prefix(ref Hint hint, ref HintDisplay __instance)
        {
            Log.Warning("A hint was attempted to be shown that is not using JadeLib!", "JadeLib");
            return false;
        }
    }
}
