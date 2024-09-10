#region

using System.Linq;
using Exiled.API.Features;
using Mirror;
using UnityEngine;

#endregion

namespace JadeLib.Features.Extensions;

/// <summary>
///     A set of questionable networking extensions that can break server.
///     <remarks>Credit to Zer0Two for some code.</remarks>
/// </summary>
public static class NetworkExtensions
{
    /// <summary>
    ///     Moves a network identity object to a new location, can be extremely buggy, only works with some objects.
    ///     <remarks>Do NOT use with players.</remarks>
    /// </summary>
    /// <param name="identity">The network identity to move.</param>
    /// <param name="pos">The new position.</param>
    public static void MoveNetworkIdentityObject(this NetworkIdentity identity, Vector3 pos)
    {
        identity.gameObject.transform.position = pos;
        ObjectDestroyMessage objectDestroyMessage = new()
        {
            netId = identity.netId,
        };
        foreach (var ply in Player.List)
        {
            ply.Connection.Send(objectDestroyMessage);
            NetworkServer.SendSpawnMessage(identity, ply.Connection);
        }
    }

    /// <summary>
    ///     Moves a network identity object to a new location, can be extremely buggy, only works with some objects.
    ///     <remarks>Do NOT use with players.</remarks>
    /// </summary>
    /// <param name="identity">The network identity to move.</param>
    /// <param name="pos">The new position.</param>
    /// <param name="players">A list of players to send the change to.</param>
    public static void MoveNetworkIdentityObjectForPlayers(
        this NetworkIdentity identity,
        Vector3 pos,
        params Player[] players)
    {
        identity.gameObject.transform.position = pos;
        ObjectDestroyMessage objectDestroyMessage = new()
        {
            netId = identity.netId,
        };
        foreach (var ply in players)
        {
            ply.Connection.Send(objectDestroyMessage);
            NetworkServer.SendSpawnMessage(identity, ply.Connection);
        }
    }

    /// <summary>
    ///     Spawns a lil' mouse.
    ///     <remarks>Squeak. (Thanks Yamato)</remarks>
    /// </summary>
    /// <param name="pos">The position to spawn the squeaker at.</param>
    public static void SpawnSqueaker(Vector3 pos)
    {
        var squeakSpawner = Object.FindObjectOfType<SqueakSpawner>();
        squeakSpawner.NetworksyncSpawn = 1;

        squeakSpawner.SyncMouseSpawn(0, squeakSpawner.syncSpawn);
        var mouse = squeakSpawner.mice.First();
        mouse.GetComponent<NetworkIdentity>().MoveNetworkIdentityObject(pos);
    }

    public static void HidePlayerFromPlayers(this Player player, params Player[] targets)
    {
        foreach (var target in targets)
        {
            target.NetworkIdentity.RemoveObserver(target.Connection);
        }
    }
}