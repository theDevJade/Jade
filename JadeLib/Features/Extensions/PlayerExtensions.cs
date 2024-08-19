// <copyright file="PlayerExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using PlayerRoles;
using UnityEngine;

namespace JadeLib.Features.Extensions;

public static class PlayerExtensions
{
    public static void CopyPlayer(this Player player, Player target)
    {
        var items = player.Inventory.UserInventory.Items.Values.ToList().Select(e => new Item(e));
        var ammo = player.Inventory.UserInventory.ReserveAmmo;
        target.Role.Set(player.Role, RoleSpawnFlags.None);
        target.Teleport(player.Position);
        target.ResetInventory(items);
        target.Inventory.UserInventory.ReserveAmmo = ammo;
        player.CopyAllProperties(target, "ReferenceHub");
    }

    public static void SetScaleNoHitbox(this Player player, Vector3 scale)
    {
        player.Scale = scale;
        player.ReferenceHub.transform.localScale = new Vector3(1, scale.y, 1);
        Server.SendSpawnMessage?.Invoke(
            null,
            [player.NetworkIdentity, player.Connection]);
    }
}