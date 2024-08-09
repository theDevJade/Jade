// <copyright file="HintService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Reflection;
using Exiled.API.Features;
using Exiled.Loader;

namespace JadeLib.Features.Registers;

public class HintService : JadeFeature
{
    public override void Enable()
    {
        Log.Info("Enabling Hint System.");
        Loader.CreatePlugin(Assembly.GetAssembly(typeof(HintServiceMeow.PluginConfig)));
    }
}