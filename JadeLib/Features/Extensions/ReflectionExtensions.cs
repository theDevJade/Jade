// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#endregion

namespace JadeLib.Features.Extensions;

public static class ReflectionExtensions
{
    public static string ReadEmbeddedResource(this Assembly assembly, string resourceName)
    {
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            return null;
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static List<string> ReadEmbeddedResourceList(this Assembly assembly, string resourceName)
    {
        var lines = new List<string>();

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            return null;
        }

        using var reader = new StreamReader(stream);
        while (reader.ReadLine() is { } line)
        {
            lines.Add(line);
        }

        return lines;
    }

    public static void CopyAllProperties(this object source, object target, params string[] ignoredProperties)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        var sourceType = source.GetType();
        var targetType = target.GetType();

        var properties = from sourceProperty in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            let targetProperty = targetType.GetProperty(
                sourceProperty.Name,
                BindingFlags.Public | BindingFlags.Instance)
            where targetProperty != null
                  && sourceProperty.CanRead
                  && targetProperty.CanWrite
                  && !ignoredProperties.Contains(sourceProperty.Name)
            select new { sourceProperty, targetProperty };

        foreach (var property in properties)
        {
            var sourceValue = property.sourceProperty.GetValue(source);
            if (property.sourceProperty.PropertyType.IsClass && property.sourceProperty.PropertyType != typeof(string))
            {
                var targetValue = property.targetProperty.GetValue(target) ??
                                  Activator.CreateInstance(property.targetProperty.PropertyType);
                property.targetProperty.SetValue(target, targetValue);
                if (sourceValue != null)
                {
                    CopyAllProperties(sourceValue, targetValue);
                }
            }
            else
            {
                property.targetProperty.SetValue(target, sourceValue);
            }
        }
    }
}