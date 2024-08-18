// <copyright file="HintService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Exiled.API.Features;
using JadeLib.Features.API.Reflection;
using JadeLib.Features.Hints;
using JadeLib.Features.Hints.Display;
using JadeLib.Features.Hints.Hints.Base;
using Mirror;

namespace JadeLib.Features.Registers;

public class HintService : JadeFeature
{
    public override void Enable()
    {
        Log.Info("Enabling Hint System.");

        var featureGroup = new FeatureGroup("jadehints").Supply(new DisplayEvents());
        featureGroup.Register();
        HintScheduler.Run();
        CustomHint.ReflectiveRegister();
    }
}