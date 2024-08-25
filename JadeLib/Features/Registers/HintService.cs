// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using Exiled.API.Features;
using JadeLib.Features.Abstract.FeatureGroups;
using JadeLib.Features.Hints.Display;
using JadeLib.Features.Hints.Hints.Base;

#endregion

namespace JadeLib.Features.Registers;

internal sealed class HintService : JadeFeature
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
        CustomHint.ReflectiveRegister();
    }
}