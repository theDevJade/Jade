// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using JadeLib.Features.Positioning.RoomPoint;
using UnityEngine;

#endregion

namespace JadeLib.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class RoomPointCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var player = Player.Get(sender);

        if (player == null || (Jade.Settings.CommandPermission.RoompointPermissions != string.Empty &&
                               player.CheckPermission(Jade.Settings.CommandPermission.RoompointPermissions)))
        {
            response = "You do not have permission to use this command.";
            return false;
        }

        var cameraTransform = player.CameraTransform.transform;

        Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var raycastHit, 100f);

        var point = new RoomPointObject(raycastHit.point + (Vector3.up * 0.1f));

        response = $"\nThe position you are looking at as RoomPoint:" +
                   $"\n  RoomType: {point.roomType}" +
                   $"\n  X: {point.relativePosition.X}" +
                   $"\n  Y: {point.relativePosition.Y}" +
                   $"\n  Z: {point.relativePosition.Z}";

        return true;
    }

    public string Command { get; } = "roompoint";

    public string[] Aliases { get; } = { "rp" };

    public string Description { get; } = "Gets the local position you're looking with the camera";
}