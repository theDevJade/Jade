#region

using Exiled.Events.EventArgs.Server;
using Exiled.Events.Handlers;
using JadeLib.Features.Probability;
using JetBrains.Annotations;

#endregion

namespace JadeLib.Features.RoundEvents;

/// <summary>
///     The main class for how RoundEvents work.
/// </summary>
public static class RoundEvents
{
    /// <summary>
    ///     Gets the RoundEvent's and their probability for selection.
    /// </summary>
    public static ProbabilitySystem<RoundEvent> ProbabilityRounds { get; } = new();

    /// <summary>
    ///     Gets or sets the current <see cref="RoundEvent" />
    ///     <remarks>Can be null.</remarks>
    /// </summary>
    [CanBeNull]
    public static RoundEvent ActiveRoundEvent { get; set; }

    internal static void Initialize()
    {
        Server.RoundStarted += OnRoundStart;
        Server.RoundEnded += OnRoundEnd;
    }

    private static void OnRoundStart()
    {
        ActiveRoundEvent ??= ProbabilityRounds.GetRandomItem();

        ActiveRoundEvent?.OnEventStarted();
    }

    private static void OnRoundEnd(RoundEndedEventArgs args)
    {
        ActiveRoundEvent?.OnEventFinished();
        ActiveRoundEvent = null;
    }
}