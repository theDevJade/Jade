#region

using System;
using HarmonyLib;

#endregion

namespace JadeLib.Features.Static;

/// <summary>
///     Contains a set of utilities for the round.
/// </summary>
public static class RoundUtils
{
    /// <summary>
    ///     Represents the function to be used when calculating the chaos/ntf count for scps. Null is default.
    /// </summary>
    public static Func<int> ChaosCountFunction { get; set; } = null;
}

[HarmonyPatch(typeof(RoundSummary), nameof(RoundSummary.Network_chaosTargetCount), MethodType.Getter)]
internal static class ChaosCountPatch
{
    [HarmonyPrefix]
    private static bool Prefix(ref int __result)
    {
        if (RoundUtils.ChaosCountFunction == null)
        {
            return true;
        }

        __result = RoundUtils.ChaosCountFunction.Invoke();
        return false;
    }
}