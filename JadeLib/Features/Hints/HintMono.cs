// -----------------------------------------------------------------------
// <copyright file="HintMono.cs" company="theDevJade">
// Copyright (c) theDevJade. All rights reserved.
// Licensed under the MIT license.
// </copyright>
// -----------------------------------------------------------------------

#region

using JadeLib.Features.Hints.Display;
using UnityEngine;

#endregion

namespace JadeLib.Features.Hints;

public class HintMono : MonoBehaviour
{
    private const float Interval = 0.572f;
    private PlayerDisplay display;
    private ReferenceHub hub;

    private float timeAccumulator;

    private void Awake()
    {
        this.hub = ReferenceHub.GetHub(this.gameObject);
        this.display = PlayerDisplay.AddDisplay(this.hub, new PlayerDisplay(this.hub));
        this.timeAccumulator = 0f;
    }

    private void Update()
    {
        this.timeAccumulator += Time.deltaTime;

        if (!(this.timeAccumulator >= Interval))
        {
            return;
        }

        this.timeAccumulator = 0f;
        this.UpdateScreen();
    }

    private void OnDestroy()
    {
        PlayerDisplay.RemoveDisplay(this.hub);
    }

    private void UpdateScreen()
    {
        this.display?.ForceUpdate();
    }
}