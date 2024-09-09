// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using JadeLib.Commands;
using JadeLib.Features.Commands;
using RemoteAdmin;

#endregion

namespace JadeLib.Features.Registers;

internal sealed class Commands : JadeFeature
{
    public override void Enable()
    {
        if (!Jade.Settings.RegisterCommands)
        {
            return;
        }

        CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(new SetSizeCommand());
        CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(new RoomPointCommand());
        CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(new RatCommand());
        JadeCommandBase.ReflectiveRegister();
    }
}