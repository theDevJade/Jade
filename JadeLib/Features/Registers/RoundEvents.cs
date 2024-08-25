#region

using JadeLib.Features.RoundEvents;

#endregion

namespace JadeLib.Features.Registers;

internal sealed class RoundEventsRegister : JadeFeature
{
    public override void Enable()
    {
        if (!Jade.Settings.UseRoundEvents)
        {
            return;
        }

        RoundEvents.RoundEvents.Initialize();
        RoundEvent.ReflectiveRegister();
    }
}