#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using HarmonyLib;

#endregion

namespace JadeLib.Features.Placeholders;

[AttributeUsage(AttributeTargets.Method)]
public class Placeholders : Attribute
{
}

public static class PlaceholderPatcher
{
    public static void ApplyPatches(Assembly assembly, Harmony harmony)
    {
        var methodsToPatch = assembly
            .GetTypes()
            .SelectMany(
                t => t.GetMethods(
                    BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            .Where(m => m.GetCustomAttributes(typeof(Placeholders), false).Length > 0);

        foreach (var method in methodsToPatch)
        {
            var original = method;
            var transpiler = typeof(PlaceholderPatcher).GetMethod(
                nameof(Transpiler),
                BindingFlags.NonPublic | BindingFlags.Static);
            harmony.Patch(original, transpiler: new HarmonyMethod(transpiler));
        }
    }

    private static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions,
        ILGenerator generator)
    {
        var codes = new List<CodeInstruction>(instructions);

        for (var i = 0; i < codes.Count; i++)
        {
            if (codes[i].opcode != OpCodes.Ldstr || codes[i].operand is not string str)
            {
                continue;
            }

            var placeholders = ExtractPlaceholders(str);

            foreach (var variableInstruction in placeholders.Select(
                         placeholder => FindVariableInstruction(
                             codes,
                             i,
                             placeholder.VarName,
                             placeholder.PatternName)).Where(variableInstruction => variableInstruction != null))
            {
                // Insert the variable load before the call to PlaceholderManager.Match
                codes.Insert(i, variableInstruction.Clone());
                i++; // Adjust for the newly inserted instruction

                // Replace the Ldstr with a call to the PlaceholderManager.Match method
                codes[i].opcode = OpCodes.Call;
                codes[i].operand = typeof(PlaceholderManager).GetMethod(
                    nameof(PlaceholderManager.Match),
                    BindingFlags.Public | BindingFlags.Static);

                // Insert the original string as the first argument
                codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldstr, str));
            }
        }

        return codes.AsEnumerable();
    }

    private static CodeInstruction FindVariableInstruction(
        List<CodeInstruction> codes,
        int ldstrIndex,
        string varName,
        string patternName)
    {
        // Traverse backwards from the ldstr instruction to find the corresponding variable
        for (var i = ldstrIndex - 1; i >= 0; i--)
        {
            var instruction = codes[i];

            // Check if the instruction is storing a local variable
            if (instruction.opcode == OpCodes.Stloc || instruction.opcode == OpCodes.Stloc_S)
            {
                var local = (LocalBuilder)instruction.operand;

                if (codes[i - 1].operand?.ToString().Contains(varName) == true)
                {
                    return new CodeInstruction(OpCodes.Ldloc, local.LocalIndex);
                }
            }

            // Check if it's a direct local load instruction
            else if (instruction.opcode == OpCodes.Ldloc || instruction.opcode == OpCodes.Ldloc_S)
            {
                var local = (LocalBuilder)instruction.operand;
                if (local.LocalType.Name == varName)
                {
                    return new CodeInstruction(OpCodes.Ldloc, local.LocalIndex);
                }
            }

            // Check if it's a method call that could return the variable
            else if (instruction.opcode == OpCodes.Call || instruction.opcode == OpCodes.Callvirt)
            {
                var methodInfo = instruction.operand as MethodInfo;
                if (methodInfo != null && methodInfo.ReturnType.Name == varName)
                {
                    return new CodeInstruction(OpCodes.Call, methodInfo);
                }
            }

            // Check if it's a field load instruction
            else if (instruction.opcode == OpCodes.Ldfld)
            {
                var fieldInfo = instruction.operand as FieldInfo;
                if (fieldInfo != null && fieldInfo.Name == varName)
                {
                    return new CodeInstruction(OpCodes.Ldfld, fieldInfo);
                }
            }
        }

        // If we didn't find the variable, return null
        return null;
    }

    public static List<(string FullMatch, string PatternName, string VarName)> ExtractPlaceholders(string input)
    {
        // Use the PlaceholderPattern from PlaceholderManager
        var pattern = PlaceholderManager.PlaceholderPattern;

        var matches = Regex.Matches(input, pattern);

        return (from Match match in matches
            let fullMatch = match.Value
            let patternName = match.Groups[1].Value
            let varName = match.Groups[2].Success ? match.Groups[2].Value : string.Empty
            select (fullMatch, patternName, varName)).ToList();
    }
}