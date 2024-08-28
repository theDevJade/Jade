// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

#endregion

namespace JadeLib.Features.Positioning.RoomPoint;

[Serializable]
public class RoomPointObject
{
    public RoomType roomType = RoomType.Unknown;

    public SerializedVector3 relativePosition = Vector3.zero;

    public RoomPointObject()
    {
    }

    public RoomPointObject(RoomType type, Vector3 relative)
    {
        this.roomType = type;
        this.relativePosition = relative;
    }

    public RoomPointObject(Vector3 mapPosition) : this(Room.Get(mapPosition).Type, mapPosition)
    {
    }
}