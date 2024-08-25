#region

using JadeLib.Features.Abstract;

#endregion

namespace JadeLib.Features.RoundEvents;

/// <summary>
///     A simplistic RoundEvent, allowing you to add spice to your rounds.
/// </summary>
public abstract class RoundEvent : ModuleSystem<RoundEvent>
{
    /// <summary>
    ///     Gets the name for this <see cref="RoundEvent" />
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    ///     Gets the display name for this <see cref="RoundEvent" />
    /// </summary>
    public virtual string DisplayName => this.Name;

    /// <summary>
    ///     Gets the probability weight for this <see cref="RoundEvent" />, between 0-100.
    /// </summary>
    protected abstract float ProbabilityWeight { get; }

    /// <summary>
    ///     To be called when this event starts.
    /// </summary>
    public abstract void OnEventStarted();

    /// <summary>
    ///     To be called when this event ends.
    /// </summary>
    public abstract void OnEventFinished();

    protected override void Register()
    {
        RoundEvents.ProbabilityRounds.AddItem(this, this.ProbabilityWeight);
    }
}