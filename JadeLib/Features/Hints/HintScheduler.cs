// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

#endregion

namespace JadeLib.Features.Hints;

public static class HintScheduler
{
    internal static void EnsureInit(ReferenceHub hub)
    {
        if (hub.gameObject.GetComponent<HintMono>() == null)
        {
            hub.gameObject.AddComponent<HintMono>();
        }
    }
}