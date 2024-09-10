#region

using Exiled.API.Features;
using JadeLib.Features.Audio;

#endregion

namespace JadeLib.Features.Registers;

internal class Redirections : JadeFeature
{
    public override void Enable()
    {
        VoiceRedirections.Init();
        Log.Info("Registered Voice Redirections");
    }
}