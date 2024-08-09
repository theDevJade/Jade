﻿// <copyright file="IHint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Exiled.API.Features;
using HintServiceMeow.Core.Models.Hints;

namespace JadeLib.Hints;

/// <summary>
/// A versatile dynamic hint.
/// </summary>
public interface IHint
{
    /// <summary>
    /// Gets or sets a dictionary of all players and their respective element.
    /// </summary>
    public List<Tuple<AbstractHint, Player>> Elements { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether debugging is enabled.
    /// </summary>
    public bool Debug { get; set; }

    /// <summary>
    /// Gets or sets the identifier for this hint.
    /// </summary>
    public string UniqueIdentifier { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not this hint should tick.
    /// </summary>
    public bool ShouldTick { get; set; }

    /// <summary>
    /// The delegate to retrieve hint content.
    /// </summary>
    /// <param name="context">The current context.</param>
    /// <returns>The hint.</returns>
    public string GetContent(AbstractHint.TextUpdateArg context);

    /// <summary>
    /// If ticking is enabled, what should happen every 0.5 seconds.
    /// </summary>
    public void Tick();
}