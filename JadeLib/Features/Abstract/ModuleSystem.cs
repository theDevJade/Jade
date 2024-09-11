#region

using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;

#endregion

namespace JadeLib.Features.Abstract;

/// <summary>
///     A ModuleSystem that allows dynamic registration of all inheriting classes of <typeparamref name="T" /> via
///     reflection.
///     Classes inheriting should be abstract and declare a register method.
/// </summary>
/// <typeparam name="T">A generic reference to the abstract class inheriting the <see cref="ModuleSystem{T}" />.</typeparam>
public abstract class ModuleSystem<T>
    where T : ModuleSystem<T>
{
    /// <summary>
    ///     A list of all registered instances of the derived modules.
    /// </summary>
    private static readonly List<T> RegisteredInstances = [];

    /// <summary>
    ///     Initializes a new instance of the <see cref="ModuleSystem{T}" /> class.
    /// </summary>
    protected ModuleSystem()
    {
        Instance = (T)this;
        this.RegisterInstances();
    }

    /// <summary>
    ///     Gets or sets the instance of <typeparamref name="T" />.
    ///     <remarks>Can be null if no instances are made.</remarks>
    /// </summary>
    public static T Instance { get; protected set; }

    /// <summary>
    ///     Method to be implemented in derived classes to handle specific registration logic.
    /// </summary>
    protected abstract void Register();

    /// <summary>
    ///     Reflectively registers all derived classes of <typeparamref name="T" /> found in the loaded assemblies.
    ///     Calls the <see cref="Register" /> method on each registered instance.
    /// </summary>
    public static void ReflectiveRegister()
    {
        var counter = 0;
        foreach (var type in Jade.UsingAssemblies.SelectMany(
                         assembly => assembly.GetTypes().Where(e => !e.IsAbstract & e.IsSubclassOf(typeof(T))))
                     .Distinct(new ModuleSystemTypeComparer()))
        {
            counter++;
            var instance = Activator.CreateInstance(type) as T;
        }

        Log.Info($"ModuleSystem registered | Name: {typeof(T).Name} | Total Modules: {counter}");

        // Call the Register method on all registered instances
        foreach (var instance in RegisteredInstances)
        {
            instance.Register();
        }
    }

    /// <summary>
    ///     Registers the current instance of the module system.
    /// </summary>
    private void RegisterInstances()
    {
        RegisteredInstances.Add((T)this);
    }
}

internal class ModuleSystemTypeComparer : IEqualityComparer<Type>
{
    public bool Equals(Type x, Type y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null)
        {
            return false;
        }

        if (y is null)
        {
            return false;
        }

        return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase) && string.Equals(
            x.FullName,
            y.FullName,
            StringComparison.OrdinalIgnoreCase);
    }

    public int GetHashCode(Type obj)
    {
        return obj.GetHashCode();
    }
}