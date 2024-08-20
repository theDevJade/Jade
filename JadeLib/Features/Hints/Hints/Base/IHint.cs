// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using System.Collections.Generic;
using Exiled.API.Features;
using JadeLib.Features.Hints.Elements;

#endregion

namespace JadeLib.Features.Hints.Hints.Base;

/// <summary>
///     A versatile dynamic hint.
/// </summary>
public interface IHint
{
    /// <summary>
    ///     Gets or sets a dictionary of all players and their respective element.
    /// </summary>
    public List<Tuple<Element, Player>> Elements { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether debugging is enabled.
    /// </summary>
    public bool Debug { get; set; }

    /// <summary>
    ///     Gets or sets the identifier for this hint.
    /// </summary>
    public string UniqueIdentifier { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether or not this hint should tick.
    /// </summary>
    public bool ShouldTick { get; set; }

    /// <summary>
    ///     A function to register listeners, etc.
    /// </summary>
    public void RegisterNonModule();

    /// <summary>
    ///     The delegate to retrieve hint content.
    /// </summary>
    /// <param name="context">The current context.</param>
    /// <returns>The hint.</returns>
    public string GetContent(HintCtx context);

    /// <summary>
    ///     If ticking is enabled, what should happen every 0.5 seconds.
    /// </summary>
    public void Tick();
}