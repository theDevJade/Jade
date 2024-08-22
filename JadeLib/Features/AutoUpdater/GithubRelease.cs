namespace JadeLib.Features.AutoUpdater;

public class GitHubRelease
{
    public string TagName { get; set; }

    public GitHubAsset[] Assets { get; set; }
}

public class GitHubAsset
{
    public string BrowserDownloadUrl { get; set; }
}