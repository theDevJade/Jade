#region

using System;
using System.Linq;
using System.Reflection;
using Exiled.Loader.Features;
using HarmonyLib;
using JadeLib.Features.Extensions;

#endregion

namespace JadeLib.Patches;

[HarmonyPatch(typeof(LoaderMessages), nameof(LoaderMessages.GetMessage))]
internal static class JadeLibEnablePatch
{
    [HarmonyPostfix]
    internal static void Postfix()
    {
        var banner = Assembly.GetExecutingAssembly().ReadEmbeddedResource("JadeLib.banner.txt");
        ServerConsole.AddLog("Exiled Loaded \n" + banner, ConsoleColor.White);
        ServerConsole.AddLog("\n", ConsoleColor.White);
        ServerConsole.AddLog(
            $"Plugins currently using JadeLib: \n {Jade.UsingAssemblies.Select(e => e.FullName).Join(delimiter: "\n ")}",
            ConsoleColor.White);
    }
}