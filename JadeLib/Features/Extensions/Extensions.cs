using System;

namespace JadeLib.Features.Extensions;

public static class Extensions
{
    public static int Dplay(this float num)
    {
        return (int)Math.Round(num);
    }
}