// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using JadeLib.Features.API.Reflection.Events;

#endregion

namespace JadeLib.Features.API.Reflection;

/// <summary>
///     Represents a group of features for a plugin, managing event handlers and coroutines.
/// </summary>
public class FeatureGroup
{
    /// <summary>
    ///     A dictionary containing all feature groups identified by a key.
    /// </summary>
    public static readonly Dictionary<string, FeatureGroup> Features = new();

    /// <summary>
    ///     The key identifying this feature group.
    /// </summary>
#pragma warning disable SA1401
    public readonly string Key;
#pragma warning restore SA1401

    /// <summary>
    ///     Initializes a new instance of the <see cref="FeatureGroup" /> class.
    ///     Adds the feature group to the dictionary.
    /// </summary>
    /// <param name="key">The key the feature group should be identified by.</param>
    public FeatureGroup(string key)
    {
        this.Key = key;
        Features.Add(this.Key, this);
    }

    /// <summary>
    ///     Gets the <see cref="EventGroup" /> handling registration of event handlers.
    /// </summary>
    public EventGroup EventGroup { get; } = new();

    /// <summary>
    ///     Gets the list of events pertaining to the <see cref="EventGroup" />.
    /// </summary>
    public List<object> Events { get; private set; } = new();

    /// <summary>
    ///     Gets the list of MEC coroutines added to this <see cref="FeatureGroup" />.
    /// </summary>
    public List<Func<IEnumerator<float>>> Coroutines { get; } = new();

    /// <summary>
    ///     Gets a value indicating whether the features in this group are registered.
    /// </summary>
    public bool IsRegistered { get; private set; }

    /// <summary>
    ///     Registers all supplied features.
    /// </summary>
    public void Register()
    {
        Log.Info("Registering Feature Group: " + this.Key);
        foreach (var @event in this.Events)
        {
            this.EventGroup.AddEventHandlers(@event);
        }

        foreach (var coroutine in this.Coroutines)
        {
            CoroutineManager.StartCoroutine(coroutine, this.Key + coroutine.Method.Name);
        }

        this.IsRegistered = true;
    }

    /// <summary>
    ///     Unregisters all supplied features.
    /// </summary>
    public void Unregister()
    {
        Log.Info("Unregistering Feature Group: " + this.Key);
        this.EventGroup.RemoveEvents();
        foreach (var coroutine in this.Coroutines)
        {
            CoroutineManager.StopCoroutine(this.Key + coroutine.Method.Name);
        }

        this.IsRegistered = false;
    }

    /// <summary>
    ///     Supplies a list of events to this feature group.
    /// </summary>
    /// <param name="classes">A params list that contains all event classes to be added.</param>
    /// <returns>The current instance of <see cref="FeatureGroup" />.</returns>
    public FeatureGroup Supply(params object[] classes)
    {
        this.Events = this.Events.Concat(classes).ToList();
        return this;
    }

    /// <summary>
    ///     Supplies a coroutine to this feature group.
    /// </summary>
    /// <param name="function">The coroutine to be added.</param>
    /// <returns>The current instance of <see cref="FeatureGroup" />.</returns>
    public FeatureGroup Supply(Func<IEnumerator<float>> function)
    {
        this.Coroutines.Add(function);
        return this;
    }

    /// <summary>
    ///     Supplies multiple coroutines to this feature group.
    /// </summary>
    /// <param name="functions">The coroutines to be added.</param>
    /// <returns>The current instance of <see cref="FeatureGroup" />.</returns>
    public FeatureGroup Supply(params Func<IEnumerator<float>>[] functions)
    {
        foreach (var function in functions)
        {
            this.Coroutines.Add(function);
        }

        return this;
    }
}