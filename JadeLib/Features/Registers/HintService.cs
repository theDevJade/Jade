// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using Exiled.API.Features;
using JadeLib.Features.API.Reflection;
using JadeLib.Features.Hints;
using JadeLib.Features.Hints.Display;
using JadeLib.Features.Hints.Hints.Base;

#endregion

namespace JadeLib.Features.Registers;

public class HintService : JadeFeature
{
    public override void Enable()
    {
        if (!Jade.Settings.UseHintSystem)
        {
            return;
        }

        Log.Info("Enabling Hint System.");
        var featureGroup = new FeatureGroup("jadehints").Supply(new DisplayEvents());
        featureGroup.Register();
        HintScheduler.Run();
        CustomHint.ReflectiveRegister();
    }
}