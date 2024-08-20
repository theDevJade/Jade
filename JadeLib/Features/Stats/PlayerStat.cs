// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using JadeLib.Features.Abstract;
using JetBrains.Annotations;

#endregion

namespace JadeLib.Features.Stats;

/// <summary>
///     A statistic for a player.
/// </summary>
/// <param name="owner">The Owner of this statistic.</param>
public abstract class Stat : ModuleSystem<Stat>
{
    [CanBeNull] public readonly ReferenceHub Owner;

    protected Stat(ReferenceHub owner)
    {
        this.Owner = owner;
        this.RegisterStat();
    }

    /// <inheritdoc />
    protected override void Register()
    {
        PlayerStats.stats.Add(this);
        this.RegisterStat();
    }

    /// <summary>
    ///     Runs when registering, used to register events and such for this stat.
    /// </summary>
    protected abstract void RegisterStat();

    /// <summary>
    ///     Runs when unregistering, used to unregister events and such for this stat.
    /// </summary>
    protected abstract void UnregisterStat();
}

/// <summary>
///     A numerical statistic for a player.
/// </summary>
/// <param name="owner">The Owner of this statistic.</param>
/// <typeparam name="T">The argument to be used in the adder for this <see cref="Stat" />.</typeparam>
public abstract class NumericalStat<T>(ReferenceHub owner) : Stat(owner)
{
    /// <summary>
    ///     Gets or sets the value of this <see cref="NumericalStat{T}" />
    /// </summary>
    public virtual int Value { get; protected set; } = 0;

    /// <summary>
    ///     The handler for the statistic
    /// </summary>
    /// <param name="value">A dynamic value that is based on <typeparamref name="T" />.</param>
    public abstract void Handle(T value);
}