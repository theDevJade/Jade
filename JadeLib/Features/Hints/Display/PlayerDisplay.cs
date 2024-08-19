// <copyright file="PlayerDisplay.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace JadeLib.Features.Hints.Display;

/// <summary>
/// The display for a player.
/// </summary>
public sealed class PlayerDisplay
{
    private static Dictionary<ReferenceHub, PlayerDisplay> displays = [];

    public static IReadOnlyDictionary<ReferenceHub, PlayerDisplay> Displays => displays;

    internal static void AddDisplay(ReferenceHub hub, PlayerDisplay display)
    {
        if (!displays.ContainsKey(hub))
        {
            displays.Add(hub, display);
        }
    }

    internal static void RemoveDisplay(ReferenceHub hub)
    {
        displays.Remove(hub);
    }

    /// <summary>
    /// The owner of this <see cref="PlayerDisplay"/>
    /// </summary>
    public readonly ReferenceHub Owner;

    /// <summary>
    /// Gets the dictionary containing all available <see cref="Screen"/>s for this <see cref="PlayerDisplay"/>
    /// </summary>
    public readonly Dictionary<string, Screen> Screens = [];

    private Screen activeScreen;

    internal PlayerDisplay(ReferenceHub hub)
    {
        this.Owner = hub;
        this.ActiveScreen = new Screen(this, "default");
        AddDisplay(hub, this);
    }

    /// <summary>
    /// Gets or sets the active screen of this <see cref="PlayerDisplay"/>
    /// </summary>
    public Screen ActiveScreen
    {
        get => this.activeScreen;
        set { this.activeScreen = this.Screens.GetOrAdd(value.Identifier, () => value); }
    }

    /// <summary>
    /// Forces an update of the active <see cref="Screen"/>, ignoring the hint ratelimit.
    /// </summary>
    public void ForceUpdate()
    {
        this.ActiveScreen.ForceUpdate();
    }
}