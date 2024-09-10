#region

using System;

#endregion

namespace JadeLib.Features.Abstract.FeatureGroups.Events;

/// <summary>
///     An attribute to mark listeners.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class ListenerAttribute : Attribute
{
}