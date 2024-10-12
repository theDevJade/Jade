#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.Features;

#endregion

namespace JadeLib.Features.Abstract.FeatureGroups.Events;

/// <summary>
///     A versatile tool for dynamically and bulk registering/unregistering events.
/// </summary>
public class EventGroup
{
    private readonly List<Tuple<Delegate, object, MethodInfo>> dynamicHandlers = new();

    /// <summary>
    ///     Gets a value indicating whether events have been registered.
    /// </summary>
    public bool IsHandlerAdded { get; private set; }

    /// <summary>
    ///     Registers all methods in the specified instances marked with <see cref="ListenerAttribute" />.
    /// </summary>
    /// <param name="classes">The instances of classes containing listener methods to be registered.</param>
    public void AddEvents(params object[] classes)
    {
        foreach (var @class in classes)
        {
            this.AddEventHandlers(@class);
        }
    }

    /// <summary>
    ///     Unregisters all currently active event handlers in this event group.
    /// </summary>
    public void RemoveEvents()
    {
        this.RemoveEventHandlers();
    }

    /// <summary>
    ///     Registers all methods in the specified instance that have the [Listener] attribute.
    /// </summary>
    /// <param name="instance">The instance containing listener methods to be registered.</param>
    public void AddEventHandlers(object instance)
    {
        var methods = instance.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.GetCustomAttributes(typeof(ListenerAttribute), false).Length > 0);

        var methodsNoArgs = instance.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
            .Where(
                m => m.GetCustomAttributes(typeof(NoargListener), false).Length > 0 &&
                     m.GetCustomAttribute<NoargListener>().TargetType.GetEvents().Any())
            .Select(e => (e, e.GetCustomAttribute<NoargListener>()));

        var methodInfos = methods.ToList();

        var valueTuples = methodsNoArgs.ToList();
        Log.Info(
            $"Registering event handlers for {instance.GetType().Name}, total of {valueTuples.Count()} noarg handlers & {methodInfos.Count()} arg handlers.");

        foreach (var method in methodInfos)
        {
            var eventType = method.GetParameters().First().ParameterType;
            var eventInstance = EventScanner.Get()[eventType];
            var delegateType = typeof(CustomEventHandler<>).MakeGenericType(eventType);
            var handler = Delegate.CreateDelegate(delegateType, instance, method);
            var eventMethods = eventInstance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
            eventMethods.First(e => e.Name == "Subscribe").Invoke(eventInstance, [handler]);
            this.dynamicHandlers.Add(new Tuple<Delegate, object, MethodInfo>(handler, instance, method));
        }

        foreach (var (e, noargListener) in valueTuples)
        {
            var eventType = noargListener.TargetType;
            var eventName = eventType.GetEvent(noargListener.EventName);
            var delegateType = typeof(CustomEventHandler);
            var handler = Delegate.CreateDelegate(delegateType, instance, e) as CustomEventHandler;
            if (eventType.GetProperty(noargListener.EventName)?.GetValue(null) is not Event @event)
            {
                continue;
            }

            @event.Subscribe(handler);
        }

        this.IsHandlerAdded = true;
    }

    private static void RegisterEventsMethods(object instance, IEnumerable<MethodInfo> methodInfos)
    {
        foreach (var method in methodInfos)
        {
            var eventType = method.GetParameters().First().ParameterType;
            var eventInstance = EventScanner.Get()[eventType];
            var delegateType = typeof(CustomEventHandler<>).MakeGenericType(eventType);
            var handler = Delegate.CreateDelegate(delegateType, instance, method);
            var eventMethods = eventInstance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
            eventMethods.First(e => e.Name == "Subscribe").Invoke(eventInstance, [handler]);
        }
    }

    public static void RegisterAllEventsForMethods(Type instance)
    {
        RegisterEventsMethods(
            null,
            instance.GetMethods().Where(
                e => e.GetParameters().Count() == 1 &&
                     e.GetParameters().First().ParameterType.IsSubclassOf(typeof(IExiledEvent))));
    }

    /// <summary>
    ///     Unregisters all event handlers that were previously registered.
    /// </summary>
    private void RemoveEventHandlers()
    {
        if (!this.IsHandlerAdded)
        {
            return;
        }

        Log.Info($"Unregistering event handlers, total of {this.dynamicHandlers.Count}.");

        foreach (var handler in this.dynamicHandlers)
        {
            var eventType = handler.Item3.GetParameters().First().ParameterType;
            var eventInstance = EventScanner.Get()[eventType];
            var eventMethods = eventInstance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
            eventMethods.First(e => e.Name == "Unsubscribe").Invoke(eventInstance, [handler.Item1]);
        }

        this.dynamicHandlers.Clear();
        this.IsHandlerAdded = false;
    }
}