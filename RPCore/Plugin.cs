#region

using System;
using CommandSystem;
using Exiled.API.Features;
using JadeLib;
using JadeLib.Features.Abstract.FeatureGroups;
using RemoteAdmin;
using RPCore.RadioChannels;
using RPCore.RPNames;

#endregion

namespace RPCore
{
    public class PluginTest : Plugin<Config>
    {
        public override void OnEnabled()
        {
            Jade.Initialize(new JadeSettings
            {
                UseHintSystem = true,
                UseRoundEvents = false,
                AutoUpdate = true,
                CommandPermission = JadeSettings.Default.CommandPermission,
                InitializeFFmpeg = false, RegisterCommands = true,
            });

            new FeatureGroup("rpnames").Supply(new NamesEvents(), new RadioEvents(), new GeneralEvents()).Register();
            base.OnEnabled();
        }
    }
}

[CommandHandler(typeof(ClientCommandHandler))]
public class NameCommand : ICommand
{
    // The command name, what the user types to run it in the console
    public string Command => "name";

    // A short description of what the command does
    public string Description => "Displays your current player name.";

    // Define aliases if you want shorter or alternative command options
    public string[] Aliases => new[] { ".name" };

    // Whether or not this command can be executed remotely via the RA (Remote Admin) panel
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        // Check if the sender is a player, we don't want this to be used by non-players
        if (sender is PlayerCommandSender playerSender)
        {
            // Get the player reference from the sender
            var player = Player.Get(playerSender.ReferenceHub);

            var combinedArgs = string.Join(" ", arguments);
            player.CustomName = combinedArgs;
            player.DisplayNickname = combinedArgs;

            // Send their name as the response
            response = $"Your name is: {player.CustomName}";
            return true;
        }

        // If the sender is not a player, return a failure response
        response = "This command can only be used by players.";
        return false;
    }
}

[CommandHandler(typeof(ClientCommandHandler))]
public class CustomInfoCommand : ICommand
{
    // The command name, what the user types to run it in the console
    public string Command => "custominfo";

    // A short description of what the command does
    public string Description => "Displays your current player name.";

    // Define aliases if you want shorter or alternative command options
    public string[] Aliases => new[] { ".ci" };

    // Whether or not this command can be executed remotely via the RA (Remote Admin) panel
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        // Check if the sender is a player, we don't want this to be used by non-players
        if (sender is PlayerCommandSender playerSender)
        {
            // Get the player reference from the sender
            var player = Player.Get(playerSender.ReferenceHub);

            var combinedArgs = string.Join(" ", arguments);
            player.CustomInfo = combinedArgs;

            // Send their name as the response
            response = $"Your custom info is: {player.CustomInfo}";
            return true;
        }

        // If the sender is not a player, return a failure response
        response = "This command can only be used by players.";
        return false;
    }
}