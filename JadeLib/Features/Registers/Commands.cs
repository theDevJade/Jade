// <copyright file="Commands.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using JadeLib.Features.Commands;
using RemoteAdmin;

namespace JadeLib.Features.Registers;

public class Commands : JadeFeature
{
    public override void Enable()
    {
        CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(new SetSizeCommand());
    }
}