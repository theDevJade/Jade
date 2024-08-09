// <copyright file="Hints.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Exiled.API.Features;
using JadeLib.Hints;

namespace JadeLib.Features.Registers;

public class Hints : JadeFeature
{
    public override void Enable()
    {
        Log.Info("Registering Hints.");
        CustomHint.ReflectiveRegister();
    }
}