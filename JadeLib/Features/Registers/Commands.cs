﻿// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using JadeLib.Features.Commands;
using RemoteAdmin;

#endregion

namespace JadeLib.Features.Registers;

public class Commands : JadeFeature
{
    public override void Enable()
    {
        CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(new SetSizeCommand());
    }
}