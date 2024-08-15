// <copyright file="AudioFile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.IO;
using Exiled.API.Features;

namespace JadeLib.Features.Audio;

public sealed class AudioFile
{
    public readonly string Path;

    public AudioFile(string path)
    {
        try
        {
            path = System.IO.Path.GetFullPath(path);
        }
        catch (Exception e)
        {
            Log.Error(e.ToString());
            throw;
        }
    }
}