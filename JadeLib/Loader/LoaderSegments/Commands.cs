#region

using JadeLib.Commands;
using JadeLib.Features.Commands;
using JadeLib.Loader;
using RemoteAdmin;

#endregion

namespace JadeLib.Features.Registers;

internal sealed class Commands : LoaderSegment
{
    public override string Name { get; } = "Commands";

    public override bool Condition => Jade.Settings.RegisterCommands;

    protected override void Load()
    {
        CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(new SetSizeCommand());
        CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(new RatCommand());
        JadeCommandBase.ReflectiveRegister();
    }

    protected override void Unload()
    {
    }
}