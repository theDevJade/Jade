// <copyright file="Redirections.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Exiled.API.Features;

namespace JadeLib.Features.Registers;

internal class Redirections : JadeFeature
{
    public override void Enable()
    {
        VoiceRedirections.Init();
        Log.Info("Registered Voice Redirections");
    }
}