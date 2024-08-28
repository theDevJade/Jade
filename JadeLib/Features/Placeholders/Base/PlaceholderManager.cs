#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#endregion

namespace JadeLib.Features.Placeholders.Base;

public static class PlaceholderManager
{
    public static string PlaceholderPattern { get; } = @"%(\w+)(?:_(\w+))?%";

    public static Dictionary<Type, PlaceholderBase> Bases { get; } = [];

    public static string Match(string input, params object[] args)
    {
        var modified = input;
        var matches = Regex.Matches(input, PlaceholderPattern);

        var results = (from Match match in matches
            let fullMatch = match.Value
            let placeholderName = match.Groups[1].Value
            let varType = match.Groups[2].Success ? match.Groups[2].Value : string.Empty
            select (fullMatch, placeholderName, varType)).ToList();

        foreach (var (fullMatch, placeholderName, varType) in results)
        {
            if (Bases.Values.All(e => e.Pattern != placeholderName))
            {
                continue;
            }

            var baseType = Bases.First(e => e.Value.Pattern == placeholderName);

            object objectArgs = null;
            var value = baseType.Key.GetProperty("PasserType")?.GetValue(baseType.Value);
            if (value is not null && args.Select(e => e.GetType()).Contains(value.GetType()))
            {
                objectArgs = args.FirstOrDefault(e => e.GetType() == value.GetType());
            }

            if (baseType.Key.GetMethod("Process")?.Invoke(
                    baseType.Value,
                    objectArgs is null ? [] : [objectArgs]) is string result)
            {
                modified = modified.Replace(fullMatch, result);
            }
        }

        return modified;
    }
}