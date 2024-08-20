// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using Exiled.API.Features;
using JadeLib.Features._914;

#endregion

namespace JadeLib.Features.Registers;

internal class Register914 : JadeFeature
{
    public override void Enable()
    {
        var manager914 = new Manager914();
        Log.Info("Registered 914 recipe handler.");
    }
}