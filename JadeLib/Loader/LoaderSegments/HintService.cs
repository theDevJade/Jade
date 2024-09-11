#region

using Exiled.API.Features;
using JadeLib.Features.Abstract.FeatureGroups;
using JadeLib.Features.Hints.Display;
using JadeLib.Features.Hints.Hints.Base;
using JadeLib.Loader;

#endregion

namespace JadeLib.Features.Registers;

internal sealed class HintService : LoaderSegment
{
    public override string Name => "Hints";

    public override bool Condition => Jade.Settings.UseHintSystem;

    protected override void Load()
    {
        Log.Info("Enabling Hint System.");
        var featureGroup = new FeatureGroup("jadehints").Supply(new DisplayEvents());
        featureGroup.Register();
        CustomHint.ReflectiveRegister();
    }

    protected override void Unload()
    {
    }
}