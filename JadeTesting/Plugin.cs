using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using JadeLib;
using JadeLib.Features;
using JadeLib.Features._914;
using JadeLib.Features.Audio;
using VoiceChat;
using VoiceChat.Networking;

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