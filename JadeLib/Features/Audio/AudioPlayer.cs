using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CentralAuth;
using JadeLib.Features.Audio.Utilities;
using MEC;
using NVorbis;
using UnityEngine;
using UnityEngine.Networking;
using VoiceChat;
using VoiceChat.Codec;
using VoiceChat.Networking;

namespace JadeLib.Features.Audio;

/// <summary>
/// Handles audio playback for a specific player.
/// </summary>
public sealed class AudioPlayer : MonoBehaviour
{
    public OpusEncoder Encoder { get; } = new(VoiceChat.Codec.Enums.OpusApplicationType.Voip);

    public Queue<AudioFile> Queue { get; } = [];

    public VoiceChatChannel Channel { get; set; } = VoiceChatChannel.Proximity;

    public bool DestroyWhenFinished { get; set; } = false;

    public List<ReferenceHub> Targets { get; private set; } = [];

    private PlaybackBuffer PlaybackBuffer { get; } = new();

    private CoroutineHandle playbackCoroutine;
    private Queue<float> streamBuffer = new();
    private VorbisReader vorbisReader;
    private MemoryStream currentPlayStream;

    private byte[] EncodedBuffer { get; } = new byte[512];

    public float[] SendBuffer { get; set; }

    public float[] ReadBuffer { get; set; }

    /// <summary>
    /// Gets or sets the owner of the audio player.
    /// </summary>
    public ReferenceHub Owner { get; set; }

    /// <summary>
    /// Gets or sets the volume of the playback.
    /// </summary>
    public float Volume { get; set; } = 100f;

    private float samplesAllowed;
    private bool ready;

    /// <summary>
    /// Gets a value indicating whether playback is occurring.
    /// </summary>
    public bool IsPlaying => this.playbackCoroutine.IsRunning;

    /// <summary>
    /// Initializes the audio player.
    /// </summary>
    /// <param name="owner">The owner of the audio player.</param>
    /// <param name="volume">The volume level.</param>
    /// <param name="hubs">A list of reference hubs to send audio to.</param>
    public AudioPlayer Initialize(ReferenceHub owner, float volume = 100f, params ReferenceHub[] hubs)
    {
        this.Owner = owner;
        this.Volume = volume;
        this.Targets = hubs.ToList();
        return this;
    }

    /// <summary>
    /// Starts playing audio in the Queue.
    /// </summary>
    /// <param name="file">The <see cref="AudioFile"/> to append to the queue. Null means none.</param>
    public void Play(AudioFile file = null)
    {
        if (file != null)
        {
            this.Queue.Enqueue(file);
        }

        this.Stop();
        this.playbackCoroutine = Timing.RunCoroutine(
            this.PlaybackRoutine(),
            Segment.FixedUpdate);
    }

    /// <summary>
    /// Stops the current playback.
    /// </summary>
    public void Stop()
    {
        if (this.IsPlaying)
        {
            Timing.KillCoroutines(this.playbackCoroutine);
        }

        this.vorbisReader.Dispose();
        this.PlaybackBuffer.Dispose();
        this.currentPlayStream.Dispose();
        this.streamBuffer.Clear();
        this.ReadBuffer = [];
        this.SendBuffer = [];
        this.ready = false;
    }

    private IEnumerator<float> PlaybackRoutine()
    {
        foreach (var track in this.Queue.TakeWhile(track => this.TryLoadAudioStream(track.Path)))
        {
            this.vorbisReader = new VorbisReader(this.currentPlayStream);
            if (!this.vorbisReader.Validate())
            {
                yield break;
            }

            this.SendBuffer = new float[(SCPSLAudio.SamplesPerSecond / 5) + SCPSLAudio.HeadSamples];
            this.ReadBuffer = new float[(SCPSLAudio.SamplesPerSecond / 5) + SCPSLAudio.HeadSamples];
            while (this.vorbisReader.ReadSamples(this.ReadBuffer, 0, this.ReadBuffer.Length) > 0)
            {
                while (!this.IsPlaying)
                {
                    yield return Timing.WaitForOneFrame;
                }

                while (this.streamBuffer.Count >= this.ReadBuffer.Length)
                {
                    this.ready = true;
                    yield return Timing.WaitForOneFrame;
                }

                this.ReadBuffer.ForEach(e => this.streamBuffer.Enqueue(e));
            }
        }
    }

    private bool TryLoadAudioStream(string trackPath)
    {
        try
        {
            if (Uri.TryCreate(trackPath, UriKind.Absolute, out _))
            {
                using var www = UnityWebRequest.Get(trackPath);
                www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    throw new Exception("Failed to load audio from URL.");
                }

                this.currentPlayStream = new MemoryStream(www.downloadHandler.data);
            }
            else
            {
                this.currentPlayStream = new MemoryStream(File.ReadAllBytes(trackPath));
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }
    }

    public void Update()
    {
        if (this.Owner is null || !this.ready || !this.streamBuffer.Any())
        {
            return;
        }

        this.samplesAllowed += Time.deltaTime * SCPSLAudio.SamplesPerSecond;
        var toCopy = Mathf.Min(Mathf.FloorToInt(this.samplesAllowed), this.streamBuffer.Count);

        if (toCopy > 0)
        {
            for (var i = 0; i < toCopy; i++)
            {
                this.PlaybackBuffer.Write(this.streamBuffer.Dequeue() * (this.Volume / 100));
            }
        }

        this.samplesAllowed -= toCopy;
        while (this.PlaybackBuffer.Length >= 480)
        {
            this.PlaybackBuffer.ReadTo(this.SendBuffer, 480);
            var dataLen = this.Encoder.Encode(this.SendBuffer, this.EncodedBuffer);

            foreach (var plr in ReferenceHub.AllHubs.Where(this.ValidatePlayer))
            {
                plr.connectionToClient.Send(
                    new VoiceMessage(this.Owner, this.Channel, this.EncodedBuffer, dataLen, false));
            }
        }
    }

    private bool ValidatePlayer(ReferenceHub hub)
    {
        return this.Targets.Contains(hub) && hub.connectionToClient != null &&
               hub.authManager.InstanceMode == ClientInstanceMode.ReadyClient &&
               hub.nicknameSync.NickSet &&
               !hub.isLocalPlayer &&
               !string.IsNullOrEmpty(hub.authManager.UserId) &&
               !hub.authManager.UserId.Contains("Dummy");
    }
}