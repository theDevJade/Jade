#region

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;

#endregion

namespace JadeLib.Features.Commands;

/// <summary>
///     A simplistic way of registering commands.
/// </summary>
/// <typeparam name="TArgument">The type for passing when processing the command.</typeparam>
public abstract class JadeCommand<TArgument> : JadeCommandBase, ICommand
{
    /// <summary>
    ///     Gets the names of the command, the first being the primary name, and the rest being aliases.
    /// </summary>
    protected abstract string[] Names { get; }

    /// <summary>
    ///     Gets the type of this command.
    /// </summary>
    protected abstract CommandType CommandType { get; }

    /// <summary>
    ///     The internal executor for this <see cref="JadeCommand{TArgument}" />
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="response">The response.</param>
    /// <returns>A bool.</returns>
    public bool Execute(
        ArraySegment<string> arguments,
        ICommandSender sender,
        [UnscopedRef] out string response)
    {
        if (sender is not PlayerCommandSender playerCommandSender)
        {
            response = "This command requires to be ran by a player.";
            return false;
        }

        var player = Player.Get(playerCommandSender.ReferenceHub);
        var validateResult = this.ValidateArguments(player, arguments.ToArray());

        switch (validateResult.Valid)
        {
            case true:
                this.Process(Player.Get(playerCommandSender.ReferenceHub), validateResult.Argument, out response);
                break;
            case false:
                response = validateResult.Response;
                break;
        }

        return true;
    }

    /// <inheritdoc />
    public string Command => this.Names[0];

    /// <inheritdoc />
    public string[] Aliases => this.Names.Skip(1).ToArray();

    /// <inheritdoc />
    public abstract string Description { get; }

    /// <summary>
    ///     The function to be run when the command is executed.
    /// </summary>
    /// <param name="player">The player running the command.</param>
    /// <param name="arguments">The arguments the player passed.</param>
    /// <param name="response">The response.</param>
    protected abstract void Process(Player player, TArgument arguments, out string response);

    /// <summary>
    ///     A function to be run in order to validate the arguments.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <param name="arguments">The arguments.</param>
    /// <returns>A <see cref="Arguments" /> containing the parsed data.</returns>
    protected abstract Arguments ValidateArguments(Player player, string[] arguments);

    protected override void Register()
    {
        if (CommandProcessor.GetAllCommands().Contains(this))
        {
            return;
        }

        switch (this.CommandType)
        {
            case CommandType.RemoteAdmin:
                CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(this);
                break;
            case CommandType.GameConsole:
                QueryProcessor.DotCommandHandler.RegisterCommand(this);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    ///     The arguments used inside a command.
    /// </summary>
    /// <param name="Valid">A value indicating if the arguments are valid.</param>
    /// <param name="Argument">The custom value as the argument.</param>
    /// <param name="Response">The response if the arguments are invalid.</param>
    protected record Arguments(bool Valid, TArgument Argument, string Response = "");
}

/// <summary>
///     A command type for use in <see cref="JadeCommand{TArgument}" />s
/// </summary>
public enum CommandType
{
    /// <summary>
    ///     A remote admin command.
    /// </summary>
    RemoteAdmin,

    /// <summary>
    ///     A game console command.
    /// </summary>
    GameConsole
}