// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using HarmonyLib;
using Mirror;

#endregion

namespace JadeLib.Features.PlayerVisibility;

[HarmonyPatch(typeof(NetworkServer), nameof(NetworkServer.ShowForConnection))]
public static class HidePlayerPatch
{
    [HarmonyPrefix]
    private static bool Prefix(ref NetworkIdentity identity, ref NetworkConnection conn)
    {
        if (!PlayerHiderMirror.Predicates.TryGetValue(identity, out var value))
        {
            return true;
        }

        return !value(conn);
    }
}