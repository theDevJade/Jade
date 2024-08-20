// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using Exiled.API.Features;
using NVorbis;
using VoiceChat;

#endregion

namespace JadeLib.Features.Audio.Utilities;

public static class SCPSLAudio
{
    public static int HeadSamples { get; } = 1920;

    public static int SamplesPerSecond => VoiceChatSettings.SampleRate * VoiceChatSettings.Channels;

    public static bool Validate(this VorbisReader vorbisReader)
    {
        if (vorbisReader.Channels == 1 && vorbisReader.SampleRate == 48000)
        {
            return true;
        }

        Log.Info("Invalid audio format. Only mono 48000Hz audio is supported.");
        return false;
    }
}