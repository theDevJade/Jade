#region

using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using JadeLib.Features.Extensions;

#endregion

namespace JadeLib.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class RatCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var player = Player.Get(sender);

        if (player == null || (Jade.Settings.CommandPermission.SpawnRatPermissions != string.Empty &&
                               player.CheckPermission(Jade.Settings.CommandPermission.SpawnRatPermissions)))
        {
            response = "You do not have permission to use this command.";
            return false;
        }

        NetworkExtensions.SpawnSqueaker(player.Position);
        response = "Spawned Rat!";

        return true;
    }

    public string Command { get; } = "spawnrat";

    public string[] Aliases { get; } = { "rat" };

    public string Description { get; } = "Spawns a rat at your location";
}