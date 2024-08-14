// <copyright file="Redirections.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Exiled.API.Features;
using JadeLib.Features.Audio;

namespace JadeLib.Features.Registers;

internal class Redirections : JadeFeature
{
    public override void Enable()
    {
        VoiceRedirections.Init();
        Log.Info("Registered Voice Redirections");
    }
}