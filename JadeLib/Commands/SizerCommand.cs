// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using CommandSystem;
using Exiled.API.Features;
using JadeLib.Features.Extensions;
using RemoteAdmin;
using UnityEngine;

#endregion

namespace JadeLib.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SetSizeCommand : ICommand
{
    public string Command { get; } = "sizer";

    public string[] Aliases { get; } = { "setsize" };

    public string Description { get; } = "Sets the size of the player executing the command.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!(sender is PlayerCommandSender playerSender))
        {
            response = "This command can only be executed by a player.";
            return false;
        }

        var executor = Player.Get(playerSender.ReferenceHub);

        if (arguments.Count < 3 || arguments.Count > 4)
        {
            response = "Usage: setsize <x> <y> <z> [playerID]";
            return false;
        }

        if (!float.TryParse(arguments.At(0), out var x) ||
            !float.TryParse(arguments.At(1), out var y) ||
            !float.TryParse(arguments.At(2), out var z))
        {
            response = "Invalid size values. Please provide valid numbers for x, y, and z.";
            return false;
        }

        var target = executor; // Default to the command executor

        if (arguments.Count == 4)
        {
            if (!int.TryParse(arguments.At(3), out var playerId))
            {
                response = "Invalid Player ID. Please provide a valid numeric Player ID.";
                return false;
            }

            target = Player.Get(playerId);
            if (target == null)
            {
                response = $"Player with ID {playerId} not found.";
                return false;
            }
        }

        target.SetScaleNoHitbox(new Vector3(x, y, z));
        response = arguments.Count == 4
            ? $"Player {target.Nickname}'s size has been set to X: {x}, Y: {y}, Z: {z}."
            : $"Your size has been set to X: {x}, Y: {y}, Z: {z}.";
        return true;
    }
}