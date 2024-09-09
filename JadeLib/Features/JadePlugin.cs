#region

using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using JadeLib.Features.Abstract.FeatureGroups;
using JadeLib.Features.UtilityClasses;

#endregion

namespace JadeLib.Features;

/// <summary>
///     A plugin that utilizes JadeLib.
/// </summary>
/// <typeparam name="TConfig">The Config.</typeparam>
public abstract class JadePlugin<TConfig> : Plugin<TConfig>
    where TConfig : IConfig, new()
{
    /// <summary>
    ///     The FeatureGroup wrapped in a <see cref="NullableObject{T}" /> for this plugin.
    /// </summary>
    public static NullableObject<FeatureGroup> PluginGroup { get; protected set; } = new(null);

    /// <summary>
    ///     Gets the <see cref="JadeSettings" /> for this plugin.
    /// </summary>
    protected virtual JadeSettings Settings => JadeSettings.Default;

    /// <summary>
    ///     Gets the name of this plugin, defaulted to the name of this class.
    /// </summary>
    protected virtual string PluginName => this.GetType().Name;

    /// <inheritdoc />
    public override string Name => this.PluginName;

    /// <summary>
    ///     Gets a list of types to register listeners of.
    /// </summary>
    protected abstract List<Type> ListenersClasses { get; }

    /// <inheritdoc />
    public override void OnEnabled()
    {
        Jade.Initialize(this.Settings);
        this.OnLoad();
        base.OnEnabled();
    }

    /// <inheritdoc />
    public override void OnDisabled()
    {
        Jade.Uninitialize();
        this.OnUnload();
        base.OnDisabled();
    }

    /// <summary>
    ///     The function to be called when loading the plugin, already initializes JadeLib before.
    /// </summary>
    protected abstract void OnLoad();

    /// <summary>
    ///     The function to be called when unloading the plugin, JadeLib will be uninitialized before.
    /// </summary>
    protected abstract void OnUnload();

    /// <summary>
    ///     Registers the events for this plugin.
    /// </summary>
    protected virtual void RegisterEvents()
    {
        var transformedGroup = this.ListenersClasses.Select(Activator.CreateInstance);
        var enumerable = transformedGroup.ToList();
        if (!enumerable.Any())
        {
            return;
        }

        var group = new FeatureGroup($"jadelib-{this.PluginName}");
        group.Events.AddRange(enumerable);
        group.Register();
        PluginGroup = new NullableObject<FeatureGroup>(group);
    }

    /// <summary>
    ///     Unregisters the events for this plugin.
    /// </summary>
    protected virtual void UnregisterEvents()
    {
        if (!PluginGroup.IsNull)
        {
            PluginGroup.Value.Unregister();
        }
    }
}