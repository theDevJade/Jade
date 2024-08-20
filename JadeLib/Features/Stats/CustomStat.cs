// <copyright file="CustomStat.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Exiled.API.Features;

namespace JadeLib.Features.Stats;

/// <inheritdoc />
public abstract class CustomStat(ReferenceHub owner) : Stat(owner)
{
    /// <inheritdoc/>
    protected override void RegisterStat()
    {
        if (!((this.Owner != null) & !Player.Get(owner).DoNotTrack))
        {
            return;
        }

        var pool = this.Owner.GetStatisticsHub();
        pool.CustomStats.Add(this);
        this.RegisterCustomStat();
    }

    /// <summary>
    /// Registers the custom stat, for events and such.
    /// </summary>
    protected abstract void RegisterCustomStat();
}