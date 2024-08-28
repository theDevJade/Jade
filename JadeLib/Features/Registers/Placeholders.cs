#region

using JadeLib.Features.Placeholders.Base;

#endregion

namespace JadeLib.Features.Registers;

public class Placeholders : JadeFeature
{
    public override void Enable()
    {
        PlaceholderBase.ReflectiveRegister();
    }
}