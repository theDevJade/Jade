using JadeLib.Commands;
using JadeLib.Features.ModularSystems;
using RemoteAdmin;

namespace JadeLib.Loader.LoaderSegments;

public class ModularSystem : LoaderSegment
{
    public override string Name => "Modular Plugin Control System";

    public override bool Condition => true;

    protected override void Load()
    {
        ModularManager.RegisterAllSystems();
        CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(new ModularSystemCommand());
    }

    protected override void Unload()
    {
    }
}