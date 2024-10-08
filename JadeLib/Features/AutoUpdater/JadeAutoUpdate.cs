﻿#region

#endregion

#region

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Exiled.API.Features;
using Utf8Json;

#endregion

namespace JadeLib.Features.AutoUpdater;

public static class JadeAutoUpdate
{
    internal static async Task CheckAndUpdateDllAsync(string owner, string repo, string currentVersion, string dllPath)
    {
        Log.Info("Checking for updates...");
        var apiUrl = $"https://api.github.com/repos/{owner}/{repo}/releases/latest";
        using var client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd("request");
        try
        {
            var response = await client.GetStringAsync(apiUrl);
            var release = JsonSerializer.Deserialize<GitHubRelease>(response);
            var latestVersion = release.TagName;
            if (string.CompareOrdinal(currentVersion, latestVersion) < 0)
            {
                Console.WriteLine($"New version available: {latestVersion}. Updating...");
                var downloadUrl = release.Assets[0].BrowserDownloadUrl;
                var dllData = await client.GetByteArrayAsync(downloadUrl);
                File.WriteAllBytes(dllPath, dllData);
                Log.Info($"DLL updated to version {latestVersion}.");
            }
            else
            {
                Log.Info("You already have the latest version.");
            }
        }
        catch (Exception ex)
        {
            Log.Error($"An exception occured while Updating: {ex.Message}\n(Contact Jade)");
        }
    }
}