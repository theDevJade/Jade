// <copyright file="SCPSLAudio.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Exiled.API.Features;
using NVorbis;
using UnityEngine;
using VoiceChat;

namespace JadeLib.Features.Audio;

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