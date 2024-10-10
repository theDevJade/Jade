// Decompiled with JetBrains decompiler
// Type: SLPlayerRotation.Extensions
// Assembly: SLPlayerRotation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B3219A50-6634-4B66-9CD9-8880F702DC28
// Assembly location: C:\Users\theDevJade\Downloads\SLPlayerRotation.dll

#region

using System;
using Exiled.API.Features;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.NetworkMessages;
using UnityEngine;

#endregion

namespace JadeLib.Features.Utility;

public static class RotationUtils
{
    public static (ushort horizontal, ushort vertical) ToClientUShorts(this Quaternion rotation)
    {
        var num1 = -rotation.eulerAngles.x;
        if (num1 < -90.0)
        {
            num1 += 360f;
        }
        else if (num1 > 270.0)
        {
            num1 -= 360f;
        }

        double num2 = Mathf.Clamp(rotation.eulerAngles.y, 0.0f, 360f);
        var num3 = Mathf.Clamp(num1, -88f, 88f) + 88f;
        return ((ushort)Math.Round(num2 * 182.04167175292969), (ushort)Math.Round(num3 * 372.35794067382813));
    }

    public static void SetRotation(this Player player, Vector3 forward)
    {
        player.ReferenceHub.SetHubRotation(forward);
    }

    public static void SetHubRotation(this Player player, Quaternion rotation)
    {
        player.ReferenceHub.SetHubRotation(rotation);
    }

    public static void SetRotation(this Player player, ushort horizontal, ushort vertical)
    {
        player.ReferenceHub.SetHubRotation(horizontal, vertical);
    }

    public static void SetHubRotation(this ReferenceHub hub, Vector3 forward)
    {
        var (horizontal, vertical) = Quaternion.LookRotation(forward, Vector3.up).ToClientUShorts();
        hub.SetHubRotation(horizontal, vertical);
    }

    public static void SetHubRotation(this ReferenceHub hub, Quaternion rotation)
    {
        var (horizontal, vertical) = rotation.ToClientUShorts();
        hub.SetHubRotation(horizontal, vertical);
    }

    public static void SetHubRotation(this ReferenceHub hub, ushort horizontal, ushort vertical)
    {
        if (hub.roleManager.CurrentRole is not IFpcRole)
        {
            return;
        }

        FpcPositionMessageWriter.appliedMouseLook = (horizontal, vertical);
        FpcPositionMessageWriter.valuesToApply |= FpcPositionMessageWriter.AppliedValues.ApplyMouseLook;
        hub.connectionToClient.Send(new FpcPositionMessage(hub));
    }
}