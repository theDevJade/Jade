#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exiled.API.Features;

#endregion

namespace JadeLib.Features;

public abstract class JadeFeature
{
    public static List<JadeFeature> Modules = new();

    public static BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance;

    public abstract void Enable();

    public static void Register()
    {
        var ass = Assembly.GetAssembly(typeof(JadeFeature));
        foreach (var type in ass.GetTypes().Where(e => e.IsSubclassOf(typeof(JadeFeature))))
        {
            Log.Info(type.Name);
            var instance = Activator.CreateInstance(type) as JadeFeature;
            Modules.Add(instance);
        }

        foreach (var coreModule in Modules)
        {
            typeof(JadeFeature).GetMethod("Enable")?.Invoke(coreModule, []);
        }
    }
}