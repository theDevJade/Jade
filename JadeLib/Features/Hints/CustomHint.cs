// <copyright file="CustomHint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Exiled.API.Features;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Models.Hints;
using JadeLib.Features.Abstract;
using JadeLib.Features.API.Reflection;
using MEC;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;

namespace JadeLib.Hints;

/// <summary>
/// A extensible CustomHint.
/// </summary>
public abstract class CustomHint() : ModuleSystem<CustomHint>, IHint
{
    private bool isRegistered = false;

    /// <summary>
    /// Gets or sets feature group for this hint.
    /// </summary>
    private FeatureGroup featureGroup;

    /// <inheritdoc/>
    public List<Tuple<AbstractHint, Player>> Elements { get; set; } = [];

    /// <inheritdoc/>
    public abstract bool Debug { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the hint. Higher Y coordinate means lower position
    /// Select from 0 to 1080
    /// </summary>
    public abstract int Vertical { get; set; }

    /// <summary>
    /// Gets or sets the horizontal offset of the hint. Higher X coordinate means more to the right
    /// This value should be between -1200 to 1200 including text length
    /// </summary>
    public abstract int Horizontal { get; set; }

    /// <inheritdoc/>
    public abstract string UniqueIdentifier { get; set; }

    /// <inheritdoc/>
    public abstract bool ShouldTick { get; set; }

    /// <summary>
    /// Gets or sets the horizontal alignment of the hint.
    /// </summary>
    protected virtual HintAlignment HintAlignmentHorizontal { get; set; } = HintAlignment.Center;

    /// <summary>
    /// Gets or sets the vertical alignment of the hint.
    /// </summary>
    protected virtual HintVerticalAlign HintAlignmentVertical { get; set; } = HintVerticalAlign.Middle;

    /// <summary>
    /// Gets or sets the tick coroutine (CAN be null)
    /// </summary>
    public CoroutineHandle? TickCoroutineHandle { get; protected set; }

    /// <inheritdoc/>
    protected override void Register()
    {
        if (this.isRegistered)
        {
            return;
        }

        this.featureGroup = new FeatureGroup(this.UniqueIdentifier);
        this.SupplyEvents(ref this.featureGroup);
        this.featureGroup.Supply(this);
        if (this.ShouldTick)
        {
            this.TickCoroutineHandle = Timing.RunCoroutine(this.TickEnumerator());
        }

        this.featureGroup.Register();

        this.isRegistered = true;
    }

    /// <inheritdoc/>
    public abstract string GetContent(AbstractHint.TextUpdateArg context);

    /// <inheritdoc/>
    public abstract void Tick();

    /// <summary>
    /// A function that registers all external events relating to this hint.
    /// <param name="group">A reference to the feature group.</param>
    /// </summary>
    protected virtual void SupplyEvents(ref FeatureGroup group)
    {
    }

    /// <summary>
    /// A required utility that adds this hint to player.
    /// </summary>
    /// <param name="player">The player to add the hint to.</param>
    /// <returns>A value indicating whether the operation was sucessful.</returns>
    public virtual bool AddToPlayer(Player player)
    {
        try
        {
            var element = new Hint()
            {
                Alignment = this.HintAlignmentHorizontal,
                YCoordinateAlign = this.HintAlignmentVertical,
                AutoText = this.ContentGetter,
                XCoordinate = this.Horizontal,
                YCoordinate = this.Vertical,
            };
            this.Elements.Add(new Tuple<AbstractHint, Player>(element, player));

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// The content getter delegate.
    /// </summary>
    /// <param name="core">The display core.</param>
    /// <returns>The hint.</returns>
    private string ContentGetter(AbstractHint.TextUpdateArg core)
    {
        if (this.Debug)
        {
            Log.Info($"Requesting hint {this.UniqueIdentifier}. Is player valid? {core.Player is not null} Is Feature Group valid? {this.featureGroup is not null}");
        }

        return this.GetContent(core);
    }

    private IEnumerator<float> TickEnumerator()
    {
        for (; ;)
        {
            this.Tick();
            yield return Timing.WaitForSeconds(0.5f);
        }
    }
}