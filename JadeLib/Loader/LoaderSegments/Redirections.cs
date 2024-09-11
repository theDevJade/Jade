#region

using Exiled.API.Features;
using JadeLib.Features.Audio;
using JadeLib.Loader;

#endregion

namespace JadeLib.Features.Registers;

internal sealed class Redirections : LoaderSegment
{
    public override string Name => "Voice Redirections";

    public override bool Condition => true;

    protected override void Load()
    {
        VoiceRedirections.Init();
        Log.Info("Registered Voice Redirections");
    }

    protected override void Unload()
    {
        // Add any necessary unload logic here
    }
}