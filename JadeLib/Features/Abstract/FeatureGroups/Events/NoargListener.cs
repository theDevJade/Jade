#region

using System;

#endregion

namespace JadeLib.Features.Abstract.FeatureGroups.Events;

/// <summary>
///     A listener for <see cref="FeatureGroup" />s that allows listening to an event that does not take arguments
///     (eventargs)
/// </summary>
/// <param name="eventType">The type of the event (Located in Exiled.Events).</param>
/// <param name="eventName">The name of the event (The actual event, not the function).</param>
[AttributeUsage(AttributeTargets.Method)]
public class NoargListener(Type eventType, string eventName) : Attribute
{
    /// <summary>
    ///     Gets the target type.
    /// </summary>
    public Type TargetType { get; } = eventType;

    /// <summary>
    ///     Gets the event name.
    /// </summary>
    public string EventName { get; } = eventName;
}