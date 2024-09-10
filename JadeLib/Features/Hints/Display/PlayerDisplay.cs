#region

using System.Collections.Generic;

#endregion

namespace JadeLib.Features.Hints.Display;

/// <summary>
///     The display for a player.
/// </summary>
public sealed class PlayerDisplay
{
    private static readonly Dictionary<ReferenceHub, PlayerDisplay> displays = [];

    /// <summary>
    ///     The owner of this <see cref="PlayerDisplay" />
    /// </summary>
    public readonly ReferenceHub Owner;

    /// <summary>
    ///     Gets the dictionary containing all available <see cref="Screen" />s for this <see cref="PlayerDisplay" />
    /// </summary>
    public readonly Dictionary<string, Screen> Screens = [];

    private Screen activeScreen;

    internal PlayerDisplay(ReferenceHub hub)
    {
        this.Owner = hub;
        this.ActiveScreen = new Screen(this, "default");
        AddDisplay(hub, this);
    }

    public static IReadOnlyDictionary<ReferenceHub, PlayerDisplay> Displays => displays;

    /// <summary>
    ///     Gets or sets the active screen of this <see cref="PlayerDisplay" />
    /// </summary>
    public Screen ActiveScreen
    {
        get => this.activeScreen;
        set { this.activeScreen = this.Screens.GetOrAdd(value.Identifier, () => value); }
    }

    internal static PlayerDisplay AddDisplay(ReferenceHub hub, PlayerDisplay display)
    {
        if (!displays.ContainsKey(hub))
        {
            displays.Add(hub, display);
        }

        return displays[hub];
    }

    internal static void RemoveDisplay(ReferenceHub hub)
    {
        displays.Remove(hub);
    }

    /// <summary>
    ///     Forces an update of the active <see cref="Screen" />, ignoring the hint ratelimit.
    /// </summary>
    public void ForceUpdate()
    {
        this.ActiveScreen.ForceUpdate();
    }
}