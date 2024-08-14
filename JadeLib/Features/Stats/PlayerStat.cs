// <copyright file="PlayerStat.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using JadeLib.Features.Abstract;
using JetBrains.Annotations;

namespace JadeLib.Features.Stats;

/// <summary>
/// A statistic for a player.
/// </summary>
/// <param name="owner">The Owner of this statistic.</param>
public abstract class Stat(ReferenceHub owner) : ModuleSystem<Stat>
{
    [CanBeNull]
    public readonly ReferenceHub Owner = owner;

    /// <inheritdoc/>
    protected override void Register()
    {
        PlayerStats.stats.Add(this);
        this.RegisterStat();
    }

    /// <summary>
    /// Runs when registering, used to register events and such for this stat.
    /// </summary>
    protected abstract void RegisterStat();

    /// <summary>
    /// Runs when unregistering, used to unregister events and such for this stat.
    /// </summary>
    protected abstract void UnregisterStat();
}

/// <summary>
/// A numerical statistic for a player.
/// </summary>
/// <param name="owner">The Owner of this statistic.</param>
/// <typeparam name="T">The argument to be used in the adder for this <see cref="Stat"/>.</typeparam>
public abstract class NumericalStat<T>(ReferenceHub owner) : Stat(owner)
{
    /// <summary>
    /// Gets or sets the value of this <see cref="NumericalStat{T}"/>
    /// </summary>
    public virtual int Value { get; protected set; } = 0;

    /// <summary>
    /// The handler for the statistic
    /// </summary>
    /// <param name="value">A dynamic value that is based on <typeparamref name="T"/>.</param>
    public abstract void Handle(T value);
}