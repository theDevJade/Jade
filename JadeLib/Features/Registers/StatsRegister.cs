// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using Exiled.API.Features;
using JadeLib.Features.API.Reflection;
using JadeLib.Features.Stats;

#endregion

namespace JadeLib.Features.Registers;

public class StatsRegister : JadeFeature
{
    public override void Enable()
    {
        Log.Info("Registering Player Statistics");
        Stat.ReflectiveRegister();
        var group = new FeatureGroup("stats").Supply(new StatEvents());
        group.Register();
    }
}