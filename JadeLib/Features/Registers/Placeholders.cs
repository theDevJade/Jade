#region

using JadeLib.Features.Placeholders;

#endregion

namespace JadeLib.Features.Registers;

public class Placeholders : JadeFeature
{
    public override void Enable()
    {
        PlaceholderBase.ReflectiveRegister();
    }
}