#region

using System;
using System.Collections.Generic;
using JadeLib.Features.Abstract;

#endregion

namespace JadeLib.Loader;

public static class JadeLoader
{
    public static List<LoaderSegment> Segments { get; } = [];

    internal static void Load()
    {
        var bar = $"LOADING-|{new string('_', Segments.Count)}|-CURRENT: ";
        foreach (var loaderSegment in Segments)
        {
            ServerConsole.AddLog($"{bar}{loaderSegment.Name} LOADING?: {loaderSegment.Condition}");
            var index = bar.IndexOf('_');
            loaderSegment.LoadExposed();
            if (index != -1)
            {
                bar = bar.Remove(index, 1).Insert(index, loaderSegment.Condition ? "#" : "X");
            }

            ServerConsole.AddLog($"{bar}{loaderSegment.Name} LOADED: {loaderSegment.Condition}");
        }

        ServerConsole.AddLog(
            $"Finished loading all features for JadeLib, total of {Segments.Count} (X indicates the feature was disabled, # means it loaded.)",
            ConsoleColor.Magenta);
    }

    internal static void Unload()
    {
        Segments.ForEach(segment => segment.UnloadExposed());
    }
}

public abstract class LoaderSegment : ModuleSystem<LoaderSegment>
{
    public bool Loaded { get; private set; }

    public abstract string Name { get; }

    public abstract bool Condition { get; }

    protected override void Register()
    {
        JadeLoader.Segments.Add(this);
    }

    public void LoadExposed()
    {
        if (this.Loaded || !this.Condition)
        {
            return;
        }

        this.Load();
        this.Loaded = true;
    }

    public void UnloadExposed()
    {
        if (!this.Loaded)
        {
            return;
        }

        this.Unload();
        this.Loaded = false;
    }

    protected abstract void Load();

    protected abstract void Unload();
}