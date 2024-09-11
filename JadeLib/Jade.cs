#region

using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using JadeLib.Features.Abstract.FeatureGroups;
using JadeLib.Loader;
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

    public static BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance;

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

        UsingAssemblies.AddIfNotContains(Assembly.GetAssembly(typeof(Jade)));

        _harmony = new Harmony("jade");
        LoaderSegment.ReflectiveRegister();

        UsingAssemblies.Add(Assembly.GetCallingAssembly());

        if (Initialized)
        {
            return false;
        }

        JadeLoader.Load();

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

        JadeLoader.Unload();

        FeatureGroup.Features.ForEach(e => { e.Value.Unregister(); });
        Initialized = false;
        return true;
    }
}