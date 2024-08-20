// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;

#endregion

namespace JadeLib.Features.API.Reflection.Events;

/// <summary>
///     An attribute to mark listeners.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class ListenerAttribute : Attribute
{
}