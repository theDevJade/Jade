#region

using Exiled.API.Features;
using JadeLib.Features.Stats;
using JadeLib.Loader;

#endregion

namespace JadeLib.Features.Registers;

internal sealed class StatsRegister : LoaderSegment
{
    public override string Name => "Player Statistics System";

    public override bool Condition => true;

    protected override void Load()
    {
        Log.Info("Registering Player Statistics");
        Stat.ReflectiveRegister();
        StatManager.Register();
    }

    protected override void Unload()
    {
    }
}