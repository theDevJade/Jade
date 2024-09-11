#region

using Exiled.API.Features;
using JadeLib.Features._914;
using JadeLib.Loader;

#endregion

namespace JadeLib.Features.Registers;

internal sealed class Register914 : LoaderSegment
{
    public override string Name => "914";

    public override bool Condition => true;

    protected override void Load()
    {
        var manager914 = new Manager914();
        Log.Info("Registered 914 recipe handler.");
    }

    protected override void Unload()
    {
    }
}