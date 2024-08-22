#region

#endregion

namespace JadeLib.Features.AutoUpdater;

public static class JadeAutoUpdate
{
    // private static async Task CheckAndUpdateDllAsync(string owner, string repo, string currentVersion, string dllPath)
    // {
    //     var apiUrl = $"https://api.github.com/repos/{owner}/{repo}/releases/latest";
    //     using (var client = new HttpClient())
    //     {
    //         client.DefaultRequestHeaders.UserAgent.ParseAdd("request");
    //         try
    //         {
    //             var response = await client.GetStringAsync(apiUrl);
    //             var release = JsonSerializer.Deserialize<GitHubRelease>(response);
    //             var latestVersion = release.TagName;
    //             if (string.CompareOrdinal(currentVersion, latestVersion) < 0)
    //             {
    //                 Console.WriteLine($"New version available: {latestVersion}. Updating...");
    //                 var downloadUrl = release.Assets[0].BrowserDownloadUrl;
    //                 var dllData = await client.GetByteArrayAsync(downloadUrl);
    //                 await File.WriteAllBytesAsync(dllPath, dllData);
    //                 Log.Info($"DLL updated to version {latestVersion}.");
    //             }
    //             else
    //             {
    //                 Log.Info("You already have the latest version.");
    //             }
    //         }
    //         catch (Exception ex)
    //         {
    //             Log.Error($"An exception occured while Updating: {ex.Message}\n(Contact Jade)");
    //         }
    //     }
    // }
}