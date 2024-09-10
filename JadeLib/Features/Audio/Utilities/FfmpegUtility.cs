#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Exiled.API.Features;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using MEC;

#endregion

namespace JadeLib.Features.Audio.Utilities;

/// <summary>
///     A utility class for downloading, extracting, and using FFmpeg to convert audio files to OGG format.
/// </summary>
public static class FfmpegUtility
{
    /// <summary>
    ///     Downloads FFmpeg, extracts it, and converts the specified input file to an OGG file with 48000 sample rate and mono
    ///     channels.
    /// </summary>
    /// <param name="inputFile">The path to the input audio file.</param>
    /// <param name="outputFile">The path to the output OGG file.</param>
    /// <returns>An IEnumerator for use with MEC coroutines.</returns>
    public static IEnumerator<float> DownloadAndConvert(string inputFile, string outputFile)
    {
        var ffmpegPath = string.Empty;
        yield return Timing.WaitUntilDone(
            Timing.RunCoroutine(DownloadAndExtractFfmpegAsync(path => ffmpegPath = path)));

        if (File.Exists(ffmpegPath))
        {
            Log.Info("FFMpeg Downloaded, good!");
        }

        yield return Timing.WaitUntilDone(Timing.RunCoroutine(ConvertToOggAsync(ffmpegPath, inputFile, outputFile)));
    }

    /// <summary>
    ///     Downloads and extracts FFmpeg asynchronously.
    /// </summary>
    /// <param name="callback">A callback function that receives the path to the FFmpeg binary.</param>
    /// <returns>An IEnumerator for use with MEC coroutines.</returns>
    public static IEnumerator<float> DownloadAndExtractFfmpegAsync(Action<string> callback)
    {
        var ffmpegUrl = GetFfmpegUrl();
        var ffmpegArchive = Path.Combine(Paths.Exiled, Path.GetFileName(ffmpegUrl));
        var ffmpegExtractPath = Path.Combine(Paths.Exiled, "ffmpeg");

        if (!Directory.Exists(ffmpegExtractPath))
        {
            Log.Info("Downloading ffmpeg...");
            using (var client = new WebClient())
            {
                client.DownloadFile(new Uri(ffmpegUrl), ffmpegArchive);
            }

            yield return Timing.WaitUntilTrue(() => File.Exists(ffmpegArchive));

            Log.Info("Extracting ffmpeg...");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.Info("Detected Windows");
                ZipFile.ExtractToDirectory(ffmpegArchive, ffmpegExtractPath, null);
            }
            else
            {
                Log.Info("Detected OSX/Linux");
                ExtractTarXz(ffmpegArchive, ffmpegExtractPath);
            }

            File.Delete(ffmpegArchive);
        }

        Log.Info("Finished Downloading");

        // Find the first folder inside the extracted folder and locate the ffmpeg binary
        var firstFolder = Directory.GetDirectories(ffmpegExtractPath).First();
        var ffmpegBinary = Path.Combine(
            firstFolder,
            "bin",
            "ffmpeg" + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : string.Empty));
        Log.Info($"Binary: {ffmpegBinary}");

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            File.SetAttributes(ffmpegBinary, File.GetAttributes(ffmpegBinary) | FileAttributes.Normal);
        }

        Log.Info($"ffmpeg information \n Extracted Folder: {ffmpegExtractPath} \n Extracted Binary: {ffmpegBinary}");

        callback(ffmpegBinary);
    }

    /// <summary>
    ///     Gets the appropriate FFmpeg download URL based on the current operating system.
    /// </summary>
    /// <returns>The URL to download FFmpeg.</returns>
    private static string GetFfmpegUrl()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "https://johnvansickle.com/ffmpeg/releases/ffmpeg-release-amd64-static.tar.xz";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "https://evermeet.cx/ffmpeg/ffmpeg.zip";
        }

        throw new NotSupportedException("Operating system not supported");
    }

    /// <summary>
    ///     Extracts a .tar.xz archive to the specified extraction path.
    /// </summary>
    /// <param name="archivePath">The path to the .tar.xz archive.</param>
    /// <param name="extractPath">The path to extract the archive contents to.</param>
    private static void ExtractTarXz(string archivePath, string extractPath)
    {
        using var fs = new FileStream(archivePath, FileMode.Open, FileAccess.Read);
        using var gzipStream = new GZipInputStream(fs);
        using var tarArchive = TarArchive.CreateInputTarArchive(gzipStream, Encoding.Default);
        tarArchive.ExtractContents(extractPath);
    }

    /// <summary>
    ///     Converts the specified input audio file to an OGG file with 48000 sample rate and mono channels using FFmpeg.
    /// </summary>
    /// <param name="ffmpegPath">The path to the FFmpeg binary.</param>
    /// <param name="inputFile">The path to the input audio file.</param>
    /// <param name="outputFile">The path to the output OGG file.</param>
    /// <returns>An IEnumerator<float> for use with MEC coroutines.</returns>
    private static IEnumerator<float> ConvertToOggAsync(string ffmpegPath, string inputFile, string outputFile)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = ffmpegPath,
            Arguments = $"-i \"{inputFile}\" -ar 48000 -ac 1 \"{outputFile}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();
        while (!process.HasExited)
        {
            yield return 0;
        }
    }
}