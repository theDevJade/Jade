// <copyright file="914.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Exiled.API.Features;
using JadeLib.Features._914;

namespace JadeLib.Features.Registers;

internal class Register914 : JadeFeature
{
    public override void Enable()
    {
        var manager914 = new Manager914();
        Log.Info("Registered 914 recipe handler.");
    }
}