// <copyright file="PlayerExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using JadeLib.Features.API;
using PlayerRoles;

namespace JadeLib.Features.Extensions;

public static class PlayerExtensions
{
    public static void CopyPlayer(this Player player, Player target)
    {
        var items = player.Inventory.UserInventory.Items.Values.ToList().Select(e => new Item(e));
        var ammo = target.Inventory.UserInventory.ReserveAmmo;
        player.Role.Set(player.Role, RoleSpawnFlags.None);
        player.Teleport(player.Position);
        player.ResetInventory(items);
        player.Inventory.UserInventory.ReserveAmmo = ammo;
        player.CopyAllProperties(target, "ReferenceHub");
    }
}