using System.Collections.Generic;
using System.IO;
using System.Reflection;

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

        using Stream stream = assembly.GetManifestResourceStream(resourceName);
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
}