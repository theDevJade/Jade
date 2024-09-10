#region

using System;
using System.Collections.Generic;
using Exiled.API.Features;
using JadeLib.Features.Abstract;
using JadeLib.Features.Abstract.FeatureGroups;
using JadeLib.Features.Hints.Display;
using JadeLib.Features.Hints.Elements;
using MEC;

#endregion

namespace JadeLib.Features.Hints.Hints.Base;

/// <summary>
///     A extensible CustomHint.
/// </summary>
public abstract class CustomHint : ModuleSystem<CustomHint>, IHint
{
    /// <summary>
    ///     Gets or sets feature group for this hint.
    /// </summary>
    private FeatureGroup featureGroup;

    private bool isRegistered;

    /// <summary>
    ///     Gets or sets the position of this hint (0-1000)
    /// </summary>
    public abstract int Position { get; set; }

    /// <summary>
    ///     Gets or sets the tick coroutine (CAN be null)
    /// </summary>
    public CoroutineHandle? TickCoroutineHandle { get; protected set; }

    /// <inheritdoc />
    public List<Tuple<Element, Player>> Elements { get; set; } = [];

    /// <inheritdoc />
    public abstract bool Debug { get; set; }

    /// <inheritdoc />
    public abstract string UniqueIdentifier { get; set; }

    /// <inheritdoc />
    public abstract bool ShouldTick { get; set; }

    /// <inheritdoc />
    public void RegisterNonModule()
    {
        if (this.isRegistered)
        {
            return;
        }

        Log.Info($"Registering Custom Hint {this.UniqueIdentifier}");

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

    /// <inheritdoc />
    public abstract string GetContent(HintCtx context);

    /// <inheritdoc />
    public abstract void Tick();

    /// <summary>
    ///     A function that registers all external events relating to this hint.
    ///     <param name="group">A reference to the feature group.</param>
    /// </summary>
    protected virtual void SupplyEvents(ref FeatureGroup group)
    {
    }

    /// <summary>
    ///     A required utility that adds this hint to player.
    /// </summary>
    /// <param name="player">The player to add the hint to.</param>
    /// <returns>A value indicating whether the operation was sucessful.</returns>
    public virtual bool AddToPlayer(Player player)
    {
        try
        {
            var element = new DynamicElement(this.Position, this.ContentGetter).AddTo(player.ReferenceHub);
            this.Elements.Add(new Tuple<Element, Player>(element, player));

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///     The content getter delegate.
    /// </summary>
    /// <param name="core">The display core.</param>
    /// <returns>The hint.</returns>
    private string ContentGetter(HintCtx core)
    {
        var player = Player.Get(core.Player);
        if (this.Debug)
        {
            Log.Info(
                $"Requesting hint {this.UniqueIdentifier}. Is player valid? {player is not null} Is Feature Group valid? {this.featureGroup is not null}");
        }

        var content = this.GetContent(core);
        if (this.Debug)
        {
            Log.Info($"Hint Callback: {content}");
        }

        return content;
    }

    private IEnumerator<float> TickEnumerator()
    {
        for (;;)
        {
            this.Tick();
            yield return Timing.WaitForSeconds(0.5f);
        }
    }

    protected override void Register()
    {
        this.RegisterNonModule();
    }
}