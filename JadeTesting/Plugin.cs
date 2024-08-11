using Exiled.API.Features;
using JadeLib;

namespace JadeTesting
{
    public class PluginTest : Plugin<Config>
    {
        public override void OnEnabled()
        {
            Log.Info("Test");
            Jade.Initialize();
            base.OnEnabled();
        }
    }
}