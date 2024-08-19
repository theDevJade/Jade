using Exiled.API.Features;
using HarmonyLib;
using Hints;
using JadeLib.Features.Hints.Display;
using Hint = Hints.Hint;

namespace JadeLib.Features.Hints;

[HarmonyPatch(typeof(HintDisplay), nameof(HintDisplay.Show))]
internal static class HintPatch
{
    [HarmonyPrefix]
    private static bool Prefix(HintDisplay __instance, ref Hint hint)
    {
        HintScheduler.RunIfNot();
        if (hint is not TextHint textHint)
        {
            Log.Debug("Not text hint.");
            return false;
        }

        var plyr = Player.Get(__instance.connectionToClient);
        plyr.ReferenceHub.GetDisplay().ActiveScreen.VanillaHint.Add(textHint);

        return false;
    }
}