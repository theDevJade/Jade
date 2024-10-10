#region

using System;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.NetworkMessages;
using RelativePositioning;

#endregion

namespace JadeLib.Features.Utility;

// Decompiled with JetBrains decompiler
// Type: SLPlayerRotation.FpcPositionMessageWriter
// Assembly: SLPlayerRotation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B3219A50-6634-4B66-9CD9-8880F702DC28
// Assembly location: C:\Users\theDevJade\Downloads\SLPlayerRotation.dll

internal static class FpcPositionMessageWriter
{
    [Flags]
    public enum FpcSyncDataByteFlags : byte
    {
        Crouching = 0,
        Sneaking = 1,
        Walking = 2,
        Sprinting = Walking | Sneaking, // 0x03
        BitCustom = 128, // 0x80
        BitPosition = 64, // 0x40
        BitMouseLook = 32, // 0x20
    }

    internal static AppliedValues valuesToApply;
    internal static (ushort horizontal, ushort vertical) appliedMouseLook;
    internal static RelativePosition appliedPosition;
    internal static PlayerMovementState appliedMovementState;

    static FpcPositionMessageWriter()
    {
        Writer<FpcPositionMessage>.write = WriteFpcPositionMessage;
    }

    private static void WriteFpcPositionMessage(NetworkWriter writer, FpcPositionMessage message)
    {
        if (valuesToApply > 0)
        {
            WriteCustomFpcPositionMessage(writer, message, valuesToApply);
            valuesToApply = 0;
        }
        else
        {
            FpcServerPositionDistributor.WriteAll(message._receiver, writer);
        }
    }

    private static void WriteCustomFpcPositionMessage(
        NetworkWriter writer,
        FpcPositionMessage message,
        AppliedValues appliedValues)
    {
        var receiver = message._receiver;
        var fpcModule = ((IFpcRole)receiver.roleManager.CurrentRole).FpcModule;
        var num1 = (appliedValues & AppliedValues.ApplyMouseLook) != 0 ? 1 : 0;
        var flag1 = (appliedValues & AppliedValues.ApplyPosition) != 0;
        var flag2 = (appliedValues & AppliedValues.ApplyMovementState) != 0;
        ushort horizontal;
        ushort vertical;
        if (num1 == 0)
        {
            horizontal = 0;
            vertical = 0;
        }
        else
        {
            (horizontal, vertical) = appliedMouseLook;
        }

        var obj = flag2 ? appliedMovementState & (PlayerMovementState)3 : (object)fpcModule.CurrentMovementState;
        var relativePosition = flag1 ? appliedPosition : default;
        var num2 = (byte)obj;
        if (num1 != 0)
        {
            num2 |= 32;
        }

        if (flag1)
        {
            num2 |= 64;
        }

        if (fpcModule.IsGrounded)
        {
            num2 |= 128;
        }

        writer.Write((ushort)2);
        writer.Write(receiver._playerId);
        writer.Write(num2);
        if (flag1)
        {
            writer.WriteRelativePosition(relativePosition);
        }

        if (num1 != 0)
        {
            writer.WriteUShort(++horizontal);
            writer.WriteUShort(++vertical);
        }

        writer.Write(receiver._playerId);
        writer.Write(num2);
        if (flag1)
        {
            writer.WriteRelativePosition(relativePosition);
        }

        if (num1 == 0)
        {
            return;
        }

        ushort num3;
        writer.WriteUShort(num3 = (ushort)(horizontal - 1U));
        ushort num4;
        writer.WriteUShort(num4 = (ushort)(vertical - 1U));
    }

    [Flags]
    internal enum AppliedValues : byte
    {
        ApplyMouseLook = 1,
        ApplyPosition = 2,
        ApplyMovementState = 4,
    }
}