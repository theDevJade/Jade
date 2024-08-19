// <copyright file="DisplayExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using JadeLib.Features.Hints.Elements;

namespace JadeLib.Features.Hints.Display;

public static class DisplayExtensions
{
    public static Element AddTo(this Element element, ReferenceHub target)
    {
        return target.GetDisplay().ActiveScreen.AddElement(element);
    }

    public static PlayerDisplay GetDisplay(this ReferenceHub hub)
    {
        if (!PlayerDisplay.Displays.ContainsKey(hub))
        {
            PlayerDisplay.AddDisplay(hub, new PlayerDisplay(hub));
        }

        return PlayerDisplay.Displays[hub];
    }
}