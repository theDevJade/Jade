#region

using Exiled.API.Features;
using JadeLib.Features.Stats;

#endregion

namespace JadeLib.Features.Registers;

internal sealed class StatsRegister : JadeFeature
{
    public override void Enable()
    {
        Log.Info("Registering Player Statistics");
        Stat.ReflectiveRegister();
        StatManager.Register();
    }
}