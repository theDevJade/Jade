// <copyright file="ServerNameHint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using JadeLib.Features.Hints.Elements;
using JadeLib.Features.Hints.Hints.Base.CustomHints;

namespace JadeLib.Features.Hints.Hints.GeneralUse;

public abstract class ServerNameHint : GlobalHint
{
    public override bool Debug { get; set; } = false;

    public abstract string Name { get; set; }

    public override string GetContent(HintCtx context)
    {
        return this.Name;
    }

    public override int Position { get; set; } = 20;
}