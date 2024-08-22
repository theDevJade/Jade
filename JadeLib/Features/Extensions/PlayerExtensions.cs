// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using PlayerRoles;
using UnityEngine;

#endregion

namespace JadeLib.Features.Extensions;

public static class PlayerExtensions
{
    public static void CopyPlayer(this Player player, Player target)
    {
        var items = player.Inventory.UserInventory.Items.Values.ToList().Select(e => new Item(e));
        var ammo = player.Inventory.UserInventory.ReserveAmmo;
        target.Health = player.Health;
        target.MaxHealth = player.MaxHealth;
        target.Role.Set(player.Role, RoleSpawnFlags.None);
        target.ClearInventory();
        target.Teleport(player.Position);
        target.ResetInventory(items);
        target.Inventory.UserInventory.ReserveAmmo = ammo;
        target.HumeShield = player.HumeShield;
        target.Rotation = player.Rotation;
        switch (player.Role.Type)
        {
            case RoleTypeId.None:
                break;
            case RoleTypeId.Scp173:
                break;
            case RoleTypeId.ClassD:
                break;
            case RoleTypeId.Spectator:
                break;
            case RoleTypeId.Scp106:
                break;
            case RoleTypeId.NtfSpecialist:
                break;
            case RoleTypeId.Scp049:
                break;
            case RoleTypeId.Scientist:
                break;
            case RoleTypeId.Scp079:
                break;
            case RoleTypeId.ChaosConscript:
                break;
            case RoleTypeId.Scp096:
                break;
            case RoleTypeId.Scp0492:
                break;
            case RoleTypeId.NtfSergeant:
                break;
            case RoleTypeId.NtfCaptain:
                break;
            case RoleTypeId.NtfPrivate:
                break;
            case RoleTypeId.Tutorial:
                break;
            case RoleTypeId.FacilityGuard:
                break;
            case RoleTypeId.Scp939:
                break;
            case RoleTypeId.CustomRole:
                break;
            case RoleTypeId.ChaosRifleman:
                break;
            case RoleTypeId.ChaosMarauder:
                break;
            case RoleTypeId.ChaosRepressor:
                break;
            case RoleTypeId.Overwatch:
                break;
            case RoleTypeId.Filmmaker:
                break;
            case RoleTypeId.Scp3114:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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