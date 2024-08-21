#region

using JadeLib.Features.Abstract;
using JetBrains.Annotations;

#endregion

namespace JadeLib.Features.Stats;

public abstract partial class Stat : ModuleSystem<Stat>
{
    [CanBeNull] public readonly ReferenceHub Owner;

    public abstract float LeaderboardThreshold { get; set; }

    public abstract int LeaderboardPriority { get; set; }

    public abstract string LeaderboardMessage { get; }

    /// <summary>
    ///     Gets or sets the value of this <see cref="Stat{T}" />
    /// </summary>
    public virtual float Value { get; protected set; } = 0;
}