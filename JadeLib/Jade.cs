using HarmonyLib;
using JadeLib.Features.API.Reflection;
using Utils.NonAllocLINQ;

namespace JadeLib;

/// <summary>
/// The entry point to JadeLib.
/// </summary>
public static class Jade
{
    private static Harmony harmony;

    /// <summary>
    /// Gets a value indicating whether JadeLib is initialized or not.
    /// </summary>
    public static bool Initialized { get; private set; } = false;

    /// <summary>
    /// A function that initializes JadeLib.
    /// </summary>
    /// <returns>A bool indicating if it was successfully initialized or not.</returns>
    public static bool Initialize()
    {
        if (Initialized)
        {
            return false;
        }

        harmony = new Harmony("jadelib");
        harmony.PatchAll();
        return true;
    }

    /// <summary>
    /// A function that uninitializes JadeLib.
    /// </summary>
    /// <returns>A bool indicating if it was successfully uninitialized or not.</returns>
    public static bool Uninitialize()
    {
        if (!Initialized)
        {
            return false;
        }

        FeatureGroup.Features.ForEach(
            e =>
            {
                e.Value.Unregister();
            });

        harmony.UnpatchAll(harmony.Id);
        return true;
    }
}