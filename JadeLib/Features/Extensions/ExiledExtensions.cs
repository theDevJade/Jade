#region

using Exiled.API.Features;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using NorthwoodLib.Pools;
using UnityEngine;
using Utils.NonAllocLINQ;

#endregion

namespace JadeLib.Features.Extensions;

/// <summary>
///     Extensions for exiled.
/// </summary>
public static class ExiledExtensions
{
    /// <summary>
    ///     Gets the location of a room, that allows you to teleport to.
    /// </summary>
    /// <param name="room">The room.</param>
    /// <returns>A nullable Vector3 possibly containing the location you want.</returns>
    public static Vector3? GetTeleportLocation(this Room room)
    {
        var roomName = room.RoomName;
        var list = ListPool<Vector3>.Shared.Rent();
        foreach (var allRoomIdentifier in RoomIdentifier.AllRoomIdentifiers)
        {
            var rid = allRoomIdentifier;
            if (rid.Name != roomName)
            {
                continue;
            }

            var position1 = rid.transform.position;
            if (!DoorVariant.AllDoors.TryGetFirst(
                    x => x.Rooms.Contains(rid) && x is BreakableDoor { IgnoreRemoteAdmin: false },
                    out var first))
            {
                list.Add(position1 + Vector3.up);
            }
            else
            {
                var position2 = first.transform.position;
                var vector3 = (position1 - position2).NormalizeIgnoreY();
                list.Add(position2 + vector3 + Vector3.up);
            }
        }

        if (list.Count == 0)
        {
            ListPool<Vector3>.Shared.Return(list);
            return null;
        }

        var position = list[Random.Range(0, list.Count)];
        ListPool<Vector3>.Shared.Return(list);
        return position;
    }
}