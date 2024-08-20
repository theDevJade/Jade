// <copyright file="GeneralExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace JadeLib.Features.Extensions;

public static class GeneralExtensions
{
    private static Random rng = new();

    public static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    public static T Random<T>(this IEnumerable<T> list)
    {
        var enumerable = list.ToList();
        return enumerable[UnityEngine.Random.Range(1, enumerable.Count())];
    }
}