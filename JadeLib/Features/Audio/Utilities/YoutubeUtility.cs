﻿#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Exiled.API.Features;
using MEC;
using NAudio.Lame;
using NAudio.Wave;
using VideoLibrary;

#endregion

namespace JadeLib.Features.Audio.Utilities;

/// <summary>
///     A set of functions that provide interaction with YouTube.
/// </summary>
public static class YoutubeUtility
{
    /// <summary>
    ///     Downloads the audio of a YouTube video from the provided URL.
    /// </summary>
    /// <param name="youtubeUrl">The URL of the YouTube video.</param>
    /// <param name="outputPath">The directory where the audio file will be saved.</param>
    /// <param name="onComplete">Callback action when the download is complete, returns the file path.</param>
    /// <param name="onError">Callback action when there is an error, returns the exception message.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    /// <exception cref="ArgumentException">Thrown when the YouTube URL is invalid or the output path is null or empty.</exception>
    public static IEnumerator<float> DownloadAudioCoroutine(
        string youtubeUrl,
        string outputPath,
        Action<string> onComplete,
        Action<string> onError)
    {
        Log.Info($"Downloading audio from {youtubeUrl}");
        if (string.IsNullOrWhiteSpace(youtubeUrl))
        {
            onError?.Invoke("YouTube URL cannot be null or empty.");
            yield break;
        }

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            onError?.Invoke("Output path cannot be null or empty.");
            yield break;
        }

        Log.Info("Debug, starting video download.");
        var youTube = YouTube.Default;
        var video = youTube.GetAllVideos(youtubeUrl).FirstOrDefault(v => v.AdaptiveKind == AdaptiveKind.Audio);

        if (video == null)
        {
            Log.Info("Debug, no audio found.");
            onError?.Invoke("No audio found for the given YouTube URL.");
            yield break;
        }

        Log.Info($"Downloading video from {video.Title}...");

        var tempVideoPath = Path.Combine(outputPath, $"{video.FullName}");
        var audioFilePath = Path.Combine(outputPath, $"{Path.GetFileNameWithoutExtension(video.FullName)}.mp3");

        // Download video
        yield return Timing.WaitForOneFrame;
        File.WriteAllBytes(tempVideoPath, video.GetBytes());

        // Extract and convert audio to MP3
        yield return Timing.WaitForOneFrame;
        using (var mediaReader = new MediaFoundationReader(tempVideoPath))
        using (var mp3Writer = new LameMP3FileWriter(audioFilePath, mediaReader.WaveFormat, LAMEPreset.STANDARD))
        {
            mediaReader.CopyTo(mp3Writer);
        }

        // Clean up temporary video file
        //File.Delete(tempVideoPath);

        onComplete?.Invoke(audioFilePath);
    }
}