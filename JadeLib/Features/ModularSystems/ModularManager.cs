using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Permissions.Extensions;

namespace JadeLib.Features.ModularSystems;

public interface IModularSystem
{
    string SystemName { get; }

    string Permission { get; }

    void InternalLoad();
}

public interface IModularComponent
{
    string Name { get; }

    string Id { get; }

    string Description { get; }

    string Permission { get; }

    string LinkedProperty { get; }

    bool Enabled { get; }

    void Enable();

    void Disable();

    void SetEnabled(bool enabled, out string response);
}

public abstract class ModularSystem<TPlugin, TConfig> : IModularSystem
    where TPlugin : class, IPlugin<TConfig>
    where TConfig : IConfig
{
    public abstract string SystemName { get; }

    public abstract string Permission { get; }

    public virtual void InternalLoad()
    {
        foreach (var assembly in Jade.UsingAssemblies)
        {
            var componentTypes = assembly.GetTypes()
                .Where(
                    t => typeof(ModularComponent<TPlugin, TConfig>).IsAssignableFrom(t) && !t.IsAbstract &&
                         !t.IsInterface);

            foreach (var type in componentTypes)
            {
                if (Activator.CreateInstance(type) is not ModularComponent<TPlugin, TConfig> component)
                {
                    continue;
                }

                ModularManager.RegisterComponent(this, component);

                if (Exiled.Loader.Loader.Plugins.FirstOrDefault(p => p is TPlugin) is not TPlugin plugin)
                {
                    continue;
                }

                var config = plugin.Config;
                var property = typeof(TConfig).GetProperty(component.LinkedProperty);
                if (property != null && property.PropertyType == typeof(bool))
                {
                    var enabled = (bool)property.GetValue(config);
                    component.SetEnabled(enabled, out _);
                }
            }
        }
    }
}

public abstract class ModularComponent<TPlugin, TConfig> : IModularComponent
    where TPlugin : class, IPlugin<TConfig>
    where TConfig : IConfig
{
    public abstract string Name { get; }

    public abstract string Id { get; }

    public abstract string Description { get; }

    public abstract string Permission { get; }

    public abstract string LinkedProperty { get; }

    public bool Enabled { get; private set; }

    public abstract void Enable();

    public abstract void Disable();

    public void SetEnabled(bool enabled, out string response)
    {
        if (Exiled.Loader.Loader.Plugins.FirstOrDefault(p => p is TPlugin) is TPlugin plugin)
        {
            var config = plugin.Config;
            var property = typeof(TConfig).GetProperty(this.LinkedProperty);
            if (property != null && property.PropertyType == typeof(bool))
            {
                property.SetValue(config, enabled);
                response = $"{this.Name} has been {(enabled ? "enabled" : "disabled")}.";
            }
            else
            {
                response = $"Property '{this.LinkedProperty}' not found or is not of type bool.";
            }
        }
        else
        {
            response = $"Plugin of type {typeof(TPlugin).Name} not found.";
        }

        if (this.Enabled != enabled)
        {
            if (enabled)
            {
                this.Enable();
            }
            else
            {
                this.Disable();
            }

            this.Enabled = enabled;
        }
    }
}

public static class ModularManager
{
    public static List<IModularSystem> Systems { get; } = new();

    public static Dictionary<IModularSystem, List<IModularComponent>> SystemComponents { get; } = new();

    /// <summary>
    /// Registers all modular systems and their components by scanning loaded assemblies.
    /// </summary>
    public static void RegisterAllSystems()
    {
        Systems.Clear();
        SystemComponents.Clear();

        var assemblies = Jade.UsingAssemblies;

        foreach (var type in assemblies.Select(
                         assembly => assembly.GetTypes()
                             .Where(t => typeof(IModularSystem).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface))
                     .SelectMany(systemTypes => systemTypes))
        {
            if (Activator.CreateInstance(type) is not IModularSystem system)
            {
                continue;
            }

            system.InternalLoad();
            RegisterSystem(system);
        }
    }

    public static void RegisterSystem(IModularSystem system)
    {
        if (!Systems.Contains(system))
        {
            Systems.Add(system);
        }

        Log.Info("New System: " + system.SystemName);
    }

    public static void RegisterComponent(IModularSystem system, IModularComponent component)
    {
        if (!SystemComponents.ContainsKey(system))
        {
            SystemComponents[system] = new List<IModularComponent>();
        }

        SystemComponents[system].Add(component);

        Log.Info("New Component: " + component.Name + " for " + system.SystemName);
    }

    public static string GetModuleStatus()
    {
        var statusBuilder = new StringBuilder();

        foreach (var system in Systems)
        {
            statusBuilder.AppendLine($"{system.SystemName}:");

            if (SystemComponents.TryGetValue(system, out var components))
            {
                foreach (var component in components)
                {
                    var color = component.Enabled ? "green" : "red";
                    var statusText = component.Enabled ? "Enabled" : "Disabled";
                    statusBuilder.AppendLine(
                        $"<color={color}>- ({component.Id}) {component.Name}: {statusText} || {component.Description}</color>");
                }
            }
            else
            {
                statusBuilder.AppendLine("No components registered.");
            }

            statusBuilder.AppendLine();
        }

        return statusBuilder.ToString();
    }
}