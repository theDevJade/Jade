// <copyright file="JadeFeature.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace JadeLib.Features;

// <copyright file="CoreModule.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exiled.API.Features;

internal abstract class JadeFeature
{
    public abstract void Enable();

    public static List<JadeFeature> Modules = new();

    public static BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance;

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