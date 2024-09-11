#region

using JadeLib.Features.RoundEvents;
using JadeLib.Loader;

#endregion

namespace JadeLib.Features.Registers;

internal sealed class RoundEventsRegister : LoaderSegment
{
    public override string Name => "Round Events";

    public override bool Condition => Jade.Settings.UseRoundEvents;

    protected override void Load()
    {
        RoundEvents.RoundEvents.Initialize();
        RoundEvent.ReflectiveRegister();
    }

    protected override void Unload()
    {
    }
}