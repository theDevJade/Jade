#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.Features;
using Exiled.Events.Handlers;

#endregion

namespace JadeLib.Features.Abstract.FeatureGroups.Events;

/// <summary>
///     A utility class that scans for all available events.
/// </summary>
public static class EventScanner
{
    /// <summary>
    ///     Gets or sets a dictionary of event types and their corresponding event instances.
    /// </summary>
    private static Dictionary<Type, object> EventTypes { get; set; }

    /// <summary>
    ///     Retrieves all available event types and their corresponding instances.
    /// </summary>
    /// <returns>A dictionary containing event types as keys and their corresponding instances as values.</returns>
    public static Dictionary<Type, object> Get()
    {
        if (EventTypes != null)
        {
            return EventTypes;
        }

        EventTypes = new Dictionary<Type, object>();
        ScanEvents();
        return EventTypes;
    }

    /// <summary>
    ///     Scans the assembly for event types and populates the <see cref="EventTypes" /> dictionary.
    /// </summary>
    private static void ScanEvents()
    {
        var exiledHandlersAssembly = typeof(Player).Assembly;

        foreach (var type in exiledHandlersAssembly.GetTypes())
        {
            if (type.Namespace == null || !type.Namespace.StartsWith("Exiled.Events.Handlers"))
            {
                continue;
            }

            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            var properties = type.GetProperties(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in fields)
            {
                if (!field.FieldType.IsGenericType ||
                    field.FieldType.GetGenericTypeDefinition() != typeof(Event<>))
                {
                    continue;
                }

                var genericType = field.FieldType.GetGenericArguments().FirstOrDefault();
                if (genericType != null && typeof(EventArgs).IsAssignableFrom(genericType))
                {
                    EventTypes[genericType] = field.GetValue(null);
                }
            }

            foreach (var property in properties)
            {
                if (!property.PropertyType.IsGenericType ||
                    property.PropertyType.GetGenericTypeDefinition() != typeof(Event<>))
                {
                    continue;
                }

                var genericType = property.PropertyType.GetGenericArguments().FirstOrDefault();
                if (genericType != null && typeof(IExiledEvent).IsAssignableFrom(genericType))
                {
                    EventTypes[genericType] = property.GetValue(null);
                }
            }
        }
    }
}