#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CentralAuth;
using Exiled.API.Features;
using Exiled.API.Features.Components;
using MEC;
using Mirror;
using PlayerRoles;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

#endregion

namespace JadeLib.Features.Positioning;

/// <summary>
///     A set of various extension functions for utility purposes.
/// </summary>
public static class GeneralUtil
{
    /// <summary>
    ///     Retrieves or initializes a <see cref="KeyValuePair{TKey,TValue}" /> for a dictionary.
    /// </summary>
    /// <param name="dict">The dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value in case it isn't found.</param>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <returns>The found/initilized value.</returns>
    public static TValue GetOrInit<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        var b = dict.TryGetValue(key, out var dictValue);
        switch (b)
        {
            case true:
                return dictValue;
            case false:
                dict.Add(key, value);
                return value;
        }
    }

    public static Vector3 CenterRoom(GameObject gameObject, Vector3 localPos)
    {
        return gameObject.transform.TransformPoint(localPos);
    }

    /// <summary>
    ///     Generates a list of positions equally spaced around a circle in the XZ plane.
    /// </summary>
    /// <param name="radius">The radius of the circle (x).</param>
    /// <param name="numPoints">The number of points to generate (n).</param>
    /// <param name="center">The center position of the circle (y).</param>
    /// <returns>A list of Vector3 positions on the circle.</returns>
    public static List<Vector3> GenerateCirclePoints(float radius, int numPoints, Vector3 center)
    {
        var positions = new List<Vector3>();

        // Calculate the angle between each point in radians
        var angleStep = 2f * Mathf.PI / numPoints;

        for (var i = 0; i < numPoints; i++)
        {
            // Current angle for this point
            var angle = i * angleStep;

            // Calculate position using polar coordinates in the XZ plane
            var x = center.x + (radius * Mathf.Cos(angle));
            var z = center.z + (radius * Mathf.Sin(angle));
            var y = center.y; // Y remains constant

            positions.Add(new Vector3(x, y, z));
        }

        return positions;
    }

    /// <summary>
    ///     Creates new NPC.
    /// </summary>
    /// <param name="name">Name setted to NPC.</param>
    /// <param name="role">Role setted to NPC.</param>
    /// <param name="userID">UserID setted to NPC. Default value hides NPC from list.</param>
    /// <returns>Returns Npc.</returns>
    public static Npc CreateNPC(
        string name,
        RoleTypeId role = RoleTypeId.None,
        string userID = PlayerAuthenticationManager.DedicatedId)
    {
        var gameObject = Object.Instantiate(NetworkManager.singleton.playerPrefab);

        var npc = new Npc(gameObject)
        {
            IsNPC = true,
        };

        try
        {
            npc.ReferenceHub.roleManager.InitializeNewRole(RoleTypeId.None, RoleChangeReason.None);
        }
        catch (Exception arg)
        {
            Log.Debug($"Ignore: {arg}");
        }

        var fakeConnection = new FakeConnection(int.MaxValue);

        NetworkServer.AddPlayerForConnection(fakeConnection, gameObject);

        try
        {
            npc.ReferenceHub.authManager.UserId = string.IsNullOrEmpty(userID) ? "Dummy@localhost" : userID;
        }
        catch (Exception arg2)
        {
            Log.Debug($"Ignore: {arg2}");
        }

        npc.ReferenceHub.nicknameSync.Network_myNickSync = name;
        Player.Dictionary.Add(gameObject, npc);

        Timing.CallDelayed(0.3f, () => { npc.Role.Set(role); });

        npc.RemoteAdminPermissions = PlayerPermissions.AFKImmunity;

        try
        {
            if (userID == PlayerAuthenticationManager.DedicatedId)
            {
                npc.ReferenceHub.authManager.SyncedUserId = userID;
                try
                {
                    npc.ReferenceHub.authManager.InstanceMode = ClientInstanceMode.DedicatedServer;
                }
                catch (Exception e)
                {
                    Log.Debug($"Ignore: {e}");
                }
            }
            else
            {
                npc.ReferenceHub.authManager.UserId = userID == string.Empty ? "Dummy@localhost" : userID;
            }
        }
        catch (Exception e)
        {
            Log.Debug($"Ignore: {e}");
        }

        return npc;
    }

    public static string GetRandomNumbers(int amount)
    {
        var randomNumberString = string.Empty;

        for (var i = 0; i < amount; i++)
        {
            var randomDigit = Random.Range(0, 10); // Generates a number between 0 and 9
            randomNumberString += randomDigit.ToString();
        }

        return randomNumberString;
    }

    /// <summary>
    ///     Using Unity's Random generates a random number between 1 and 100.
    /// </summary>
    /// <param name="number">The chance to return true.</param>
    /// <returns>If it was true or not.</returns>
    public static bool Chance(this int number)
    {
        var random = Random.Range(1, 100);
        return random <= number;
    }

    public static void CopyPublicFields(object source, object destination)
    {
        if (source == null || destination == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var sourceType = source.GetType();
        var destinationType = destination.GetType();

        var sourceFields = sourceType.GetFields(BindingFlags.Public | BindingFlags.Instance);
        var destinationFields = destinationType.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var sourceField in sourceFields)
        {
            var destinationField = Array.Find(
                destinationFields,
                f => f.Name == sourceField.Name && f.FieldType == sourceField.FieldType);
            if (destinationField != null)
            {
                destinationField.SetValue(destination, sourceField.GetValue(source));
            }
        }
    }

    public static void GetOrNothing<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Action<TValue> func)
    {
        var b = dict.TryGetValue(key, out var value);
        switch (b)
        {
            case true:
                func.Invoke(value);
                break;
            case false:
                break;
        }
    }

    public static bool ContainsTuple<TKey, TValue>(this IEnumerable<Tuple<TKey, TValue>> enumerable, TKey check)
    {
        var comparer = EqualityComparer<TKey>.Default;
        return enumerable.Any(e => comparer.Equals(e.Item1, check));
    }

    public static bool ContainsTuple<TKey, TValue>(this IEnumerable<Tuple<TKey, TValue>> enumerable, TValue check)
    {
        var comparer = EqualityComparer<TValue>.Default;
        return enumerable.Any(e => comparer.Equals(e.Item2, check));
    }
}