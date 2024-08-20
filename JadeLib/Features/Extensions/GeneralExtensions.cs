// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace JadeLib.Features.Extensions;

public static class GeneralExtensions
{
    private static readonly Random rng = new();

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