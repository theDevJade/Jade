// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using Exiled.API.Features;

#endregion

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