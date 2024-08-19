// <copyright file="SizerCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using JadeLib.Features.Extensions;
using RemoteAdmin;

namespace JadeLib.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SetSizeCommand : ICommand
{
    public string Command { get; } = "sizer";

    public string[] Aliases { get; } = new string[] { "setsize" };

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

        target.SetScaleNoHitbox(new UnityEngine.Vector3(x, y, z));
        response = arguments.Count == 4
            ? $"Player {target.Nickname}'s size has been set to X: {x}, Y: {y}, Z: {z}."
            : $"Your size has been set to X: {x}, Y: {y}, Z: {z}.";
        return true;
    }
}