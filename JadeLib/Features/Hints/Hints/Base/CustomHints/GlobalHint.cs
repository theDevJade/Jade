// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using Exiled.Events.EventArgs.Player;
using JadeLib.Features.API.Reflection.Events;

#endregion

namespace JadeLib.Features.Hints.Hints.Base.CustomHints;

/// <inheritdoc />
public abstract class GlobalHint : CustomHint
{
    /// <inheritdoc />
    public abstract override string UniqueIdentifier { get; set; }

    /// <inheritdoc />
    public override bool ShouldTick { get; set; } = false;

    /// <inheritdoc />
    public override void Tick()
    {
    }

    [Listener]
    public void OnVerify(VerifiedEventArgs args)
    {
        this.AddToPlayer(args.Player);
    }
}