// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Reflection;
using Exiled.API.Features;
using HarmonyLib;
using JadeLib.Features;
using JadeLib.Features.Abstract.FeatureGroups;
using JadeLib.Features.Audio.Utilities;
using JadeLib.Features.Extensions;
using JadeLib.Features.Placeholders;
using MEC;
using Utils.NonAllocLINQ;

#endregion

namespace JadeLib;

/// <summary>
///     The entry point to JadeLib.
/// </summary>
public static class Jade
{
    internal static Harmony _harmony;

    internal static List<Assembly> UsingAssemblies = [];

    /// <summary>
    ///     Gets a value indicating whether JadeLib is initialized or not.
    /// </summary>
    public static bool Initialized { get; private set; }

    public static JadeSettings Settings { get; set; } = JadeSettings.Default;

    /// <summary>
    ///     A function that initializes JadeLib.
    /// </summary>
    /// <param name="settings">The settings to initialize with, defaults to. <code>JadeSettings.Default</code></param>
    /// <returns>A bool indicating if it was successfully initialized or not.</returns>
    public static bool Initialize(JadeSettings settings = null)
    {
        settings ??= Settings;

        Settings = settings;
        _harmony = new Harmony("jade");

        var callingAssembly = Assembly.GetCallingAssembly();
        if (!UsingAssemblies.Contains(callingAssembly))
        {
            PlaceholderPatcher.ApplyPatches(callingAssembly, _harmony);
        }

        UsingAssemblies.Add(Assembly.GetCallingAssembly());
        UsingAssemblies.AddIfNotContains(Assembly.GetAssembly(typeof(Jade)));
        if (Initialized)
        {
            return false;
        }

        Log.Info("Initializing JadeLib");
        CosturaUtility.Initialize();
        Log.Info("Initialized Embedded DLL's");

        _harmony.PatchAll();
        Log.Info("All harmony patches have been applied");

        if (Settings.InitializeFFmpeg)
        {
            Timing.RunCoroutine(FfmpegUtility.DownloadAndExtractFfmpegAsync(Log.Info));
            Log.Info("Ffmpeg has been installed.");
        }

        JadeFeature.Register();

        var banner = Assembly.GetExecutingAssembly().ReadEmbeddedResource("JadeLib.banner.txt");
        ServerConsole.AddLog("JadeLib is ready to go! \n" + banner, ConsoleColor.White);

        Initialized = true;

        return true;
    }

    /// <summary>
    ///     A function that uninitializes all feature groups.
    /// </summary>
    /// <returns>A bool indicating if it was successfully uninitialized or not.</returns>
    public static bool Uninitialize()
    {
        if (!Initialized)
        {
            return false;
        }

        _harmony.UnpatchAll(_harmony.Id);

        FeatureGroup.Features.ForEach(e => { e.Value.Unregister(); });

        _harmony.UnpatchAll(_harmony.Id);
        Initialized = false;
        return true;
    }
}