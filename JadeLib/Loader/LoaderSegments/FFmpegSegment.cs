#region

using Exiled.API.Features;
using JadeLib.Features.Audio.Utilities;
using MEC;

#endregion

namespace JadeLib.Loader.LoaderSegments;

public class FFmpegSegment : LoaderSegment
{
    public override string Name { get; } = "FFmpeg";

    public override bool Condition => Jade.Settings.InitializeFFmpeg;

    protected override void Load()
    {
        Timing.RunCoroutine(FfmpegUtility.DownloadAndExtractFfmpegAsync(Log.Info));
    }

    protected override void Unload()
    {
    }
}