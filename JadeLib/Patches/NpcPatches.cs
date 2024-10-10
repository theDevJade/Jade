#region

using System;
using System.Linq;
using CentralAuth;
using Exiled.API.Features;
using HarmonyLib;

#endregion

namespace JadeLib.Patches;

/// <summary>
///     Fix NPC changing instace mode to not cause issued with VSR.
/// </summary>
[HarmonyPatch(typeof(PlayerAuthenticationManager), nameof(PlayerAuthenticationManager.InstanceMode), MethodType.Setter)]
public static class FixNpcInstanceMode
{
    /// <summary>
    ///     ....
    /// </summary>
    /// <param name="__instance">..</param>
    /// <param name="value">.</param>
    /// <returns>.//.</returns>
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
    public static bool Prefix(PlayerAuthenticationManager __instance, ClientInstanceMode value)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
    {
        if (!Player.TryGet(__instance._hub, out var npc) || !npc.IsNPC)
        {
            return true;
        }

        if (value != ClientInstanceMode.Unverified && value != ClientInstanceMode.Host &&
            value != ClientInstanceMode.DedicatedServer)
        {
            Log.Info($"Prevented NPC [{npc.Id}] changing Instance mode to {value} from {npc.ReferenceHub.Mode}");
            return false;
        }

        return true;
    }
}

/// <summary>
///     Fix NPC counts as players **IN** server list.
/// </summary>
[HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.RefreshOnlinePlayers))]
public static class FixNpcServerListDisplay
{
    /// <summary>
    ///     ....
    /// </summary>
    /// .
    /// <returns>.//.</returns>
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
    public static bool Prefix()
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
    {
        try
        {
            foreach (var allHub in ReferenceHub.AllHubs)
            {
                if (!Player.TryGet(allHub, out var player))
                {
                    continue;
                }

                if (player.IsNPC)
                {
                    continue;
                }

                if (allHub.Mode == ClientInstanceMode.ReadyClient && !string.IsNullOrEmpty(allHub.authManager.UserId) &&
                    (!allHub.isLocalPlayer || !ServerStatic.IsDedicated))
                {
                    ServerConsole.PlayersListRaw.objects.Add(allHub.authManager.UserId);
                }
            }

            ServerConsole._verificationPlayersList = JsonSerialize.ToJson(ServerConsole.PlayersListRaw);

            ServerConsole._playersAmount = Player.List.Count(x => !x.IsNPC);

            ServerConsole.PlayersListRaw.objects.Clear();
        }
        catch (Exception ex)
        {
            ServerConsole.AddLog("[VERIFICATION] Exception in Players Online processing: " + ex.Message);
            ServerConsole.AddLog(ex.StackTrace);
        }

        return false;
    }
}

/// <summary>
///     Fixs problems with ID.
/// </summary>
[HarmonyPatch(typeof(ReferenceHub), nameof(ReferenceHub.Network_playerId), MethodType.Setter)]
public static class IDFix
{
    /// <summary>
    ///     Gets or sets auto Increment ID.
    /// </summary>
    public static int AutoIncrement { get; set; }

    /// <summary>
    ///     xz.
    /// </summary>
    /// <param name="value">xzzzz.</param>
    [HarmonyPrefix]
    public static void Prefix(ref RecyclablePlayerId value)
    {
        value.Destroy();
        value = new RecyclablePlayerId(++AutoIncrement);
    }

    /// <summary>
    ///     Refresh ID on round restart.
    /// </summary>
    public static void OnRestartRound_RefreshID()
    {
        AutoIncrement = 0;
    }
}