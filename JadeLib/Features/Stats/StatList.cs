// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System.Collections.Generic;
using JadeLib.Features.UtilityClasses;

#endregion

namespace JadeLib.Features.Stats;

public sealed class CustomList<T> : List<T>
{
    public NullableObject<T> Get(T type)
    {
        this.TryGet(this.IndexOf(type), out var obj);
        var objCast = new NullableObject<T>(obj);
        return objCast;
    }
}