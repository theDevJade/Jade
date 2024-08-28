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
        CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(new SetSizeCommand());
        JadeCommandBase.ReflectiveRegister();
    }
}