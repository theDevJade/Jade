using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CentralAuth;
using Exiled.API.Features;
using MEC;
using Mirror;
using NVorbis;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using VoiceChat;
using VoiceChat.Codec;
using VoiceChat.Networking;
using Random = UnityEngine.Random;

namespace JadeLib.Features.Audio;

public sealed class AudioPlayer : MonoBehaviour
{
    public static readonly Dictionary<ReferenceHub, AudioPlayer> AudioPlayers = new();

    #region Internal

    internal AudioPlayer()
    {
    }

    private const int HeadSamples = 1920;

    private OpusEncoder Encoder { get; } = new(VoiceChat.Codec.Enums.OpusApplicationType.Voip);

    private PlaybackBuffer PlaybackBuffer { get; } = new();

    private byte[] EncodedBuffer { get; } = new byte[512];

    public bool stopTrack;
    public bool ready;
    private CoroutineHandle _playbackCoroutine;

    public float allowedSamples;
    public int samplesPerSecond;

    private Queue<float> StreamBuffer { get; } = new();

    public VorbisReader VorbisReader { get; set; }

    public float[] SendBuffer { get; set; }

    public float[] ReadBuffer { get; set; }

    #endregion

    #region AudioPlayer Settings

    /// <summary>
    /// The ReferenceHub instance that this player sends as.
    /// </summary>
    public ReferenceHub Owner { get; set; }

    /// <summary>
    /// Volume that the player will play at.
    /// </summary>
    public float Volume { get; set; } = 100f;

    /// <summary>
    /// List of Paths/Urls that the player will play from (Urls only work if <see cref="allowUrl"/> is true)
    /// </summary>
    [FormerlySerializedAs("AudioToPlay")] public List<string> audioToPlay = [];

    /// <summary>
    /// Path/Url of the currently playing audio file.
    /// </summary>
    [FormerlySerializedAs("CurrentPlay")] public string currentPlay;

    /// <summary>
    /// Stream containing the Audio data
    /// </summary>
    private MemoryStream _currentPlayStream;

    /// <summary>
    /// Boolean indicating whether or not the Queue will loop (Audio will be added to the end of the queue after it gets removed on play)
    /// </summary>
    [FormerlySerializedAs("Loop")] public bool loop;

    /// <summary>
    /// If the playlist should be shuffled when an audio track start.
    /// </summary>
    [FormerlySerializedAs("Shuffle")] public bool shuffle;

    /// <summary>
    /// Whether the Player should continue playing by itself after the current Track ends.
    /// </summary>
    [FormerlySerializedAs("Continue")] public bool @continue = true;

    /// <summary>
    /// Whether the Player should be sending audio to the broadcaster.
    /// </summary>
    [FormerlySerializedAs("ShouldPlay")] public bool shouldPlay = true;

    /// <summary>
    /// If URLs are allowed to be played
    /// </summary>
    [FormerlySerializedAs("AllowUrl")] public bool allowUrl;

    /// <summary>
    /// Determines whether debug logs should be shown. Note: Enabling this option can generate a large amount of log output.
    /// </summary>
    [FormerlySerializedAs("LogDebug")] public bool logDebug;

    /// <summary>
    /// Determines whether informational logs should be shown throughout the code.
    /// </summary>
    [FormerlySerializedAs("LogInfo")] public bool logInfo;

    /// <summary>
    /// Gets a value indicating whether the current song has finished playing.
    /// </summary>
    [FormerlySerializedAs("IsFinished")] public bool isFinished;

    /// <summary>
    /// Determines whether the ReferenceHub will be destroyed after finishing playing all tracks.
    /// </summary>
    [FormerlySerializedAs("ClearOnFinish")]
    public bool clearOnFinish = true;

    /// <summary>
    /// If not empty, the audio will only be sent to players with the PlayerIds in this list
    /// </summary>
    [FormerlySerializedAs("BroadcastTo")] public List<int> broadcastTo = [];

    /// <summary>
    /// Gets or Sets the Channel where audio will be played in
    /// </summary>
    public VoiceChatChannel BroadcastChannel { get; set; } = VoiceChatChannel.Proximity;

    public Audio Will;

    #endregion

    #region Events

    /// <summary>
    /// Fired when a track is getting selected.
    /// </summary>
    /// <param name="player">The AudioPlayer instance that this event fired for</param>
    /// <param name="directPlay">If the AudioPlayer was playing Directly (-1 index)</param>
    /// <param name="queuePos">Position in the Queue of the track that is going to be selected</param>
    public delegate void TrackSelecting(AudioPlayer player, bool directPlay, ref int queuePos);

    public static event TrackSelecting OnTrackSelecting;

    /// <summary>
    /// Fired when a track has been selected
    /// </summary>
    /// <param name="player">The AudioPlayer instance that this event fired for</param>
    /// <param name="directPlay">If the AudioPlayer was playing Directly (-1 index)</param>
    /// <param name="queuePos">Position in the Queue of the track that will start</param>
    /// <param name="track">The track the AudioPlayer will play</param>
    public delegate void TrackSelected(AudioPlayer player, bool directPlay, int queuePos, ref string track);

    public static event TrackSelected OnTrackSelected;

    /// <summary>
    /// Fired when a track is loaded and will begin playing.
    /// </summary>
    /// <param name="player">The AudioPlayer instance that this event fired for</param>
    /// <param name="directPlay">If the AudioPlayer was playing Directly (-1 index)</param>
    /// <param name="queuePos">Position in the Queue that will play</param>
    /// <param name="track">The track the AudioPlayer will play</param>
    public delegate void TrackLoaded(AudioPlayer player, bool directPlay, int queuePos, string track);

    public static event TrackLoaded OnTrackLoaded;

    /// <summary>
    /// Fired when a track finishes.
    /// </summary>
    /// <param name="player">The AudioPlayer instance that this event fired for</param>
    /// <param name="track">The track the AudioPlayer was playing</param>
    /// <param name="directPlay">If the AudioPlayer was playing Directly (-1 index)</param>
    /// <param name="nextQueuePos">Position in the Queue that will play next, can be set to a different value</param>
    public delegate void TrackFinished(AudioPlayer player, string track, bool directPlay, ref int nextQueuePos);

    public static event TrackFinished OnFinishedTrack;

    #endregion

    /// <summary>
    /// Add or retrieve the AudioPlayer instance based on a ReferenceHub instance.
    /// </summary>
    /// <param name="hub">The ReferenceHub instance that this AudioPlayer belongs to</param>
    /// <returns><see cref="AudioPlayer"/></returns>
    internal static AudioPlayer GetOrCreate(ReferenceHub hub, Audio audio)
    {
        if (AudioPlayers.TryGetValue(hub, out var player))
        {
            return player;
        }

        player = hub.gameObject.AddComponent<AudioPlayer>();
        player.Owner = hub;
        player.Will = audio;

        AudioPlayers.Add(hub, player);
        return player;
    }

    /// <summary>
    /// Start playing audio, if called while audio is already playing the player will skip to the next file.
    /// </summary>
    /// <param name="queuePos">The position in the queue of the audio that should be played.</param>
    public void Play(int queuePos)
    {
        if (this._playbackCoroutine.IsRunning)
        {
            Timing.KillCoroutines(this._playbackCoroutine);
        }

        this._playbackCoroutine = Timing.RunCoroutine(this.Playback(queuePos), Segment.FixedUpdate);
    }

    /// <summary>
    /// Stops playing the current Track, or stops the player entirely if Clear is true.
    /// </summary>
    /// <param name="clear">If true the player will stop and the queue will be cleared.</param>
    public void Stop(bool clear)
    {
        if (clear)
        {
            this.audioToPlay.Clear();
        }

        this.stopTrack = true;
    }

    /// <summary>
    /// Add an audio file to the queue
    /// </summary>
    /// <param name="audio">Path/Url to an audio file (Url only works if <see cref="allowUrl"/> is true)</param>
    /// <param name="pos">Position that the audio file should be inserted at, use -1 to insert at the end of the queue.</param>
    public void Enqueue(string audio, int pos)
    {
        if (pos == -1)
        {
            this.audioToPlay.Add(audio);
        }
        else
        {
            this.audioToPlay.Insert(pos, audio);
        }
    }

    public void OnDestroy()
    {
        if (this._playbackCoroutine.IsRunning)
        {
            Timing.KillCoroutines(this._playbackCoroutine);
        }

        AudioPlayers.Remove(this.Owner);

        if (this.clearOnFinish)
        {
            NetworkServer.RemovePlayerForConnection(this.Owner.connectionToClient, true);
        }
    }

    private IEnumerator<float> Playback(int position)
    {
        this.stopTrack = false;
        this.isFinished = false;
        var index = position;

        OnTrackSelecting?.Invoke(this, index == -1, ref index);
        if (index != -1)
        {
            if (this.shuffle)
            {
                this.audioToPlay = this.audioToPlay.OrderBy(i => Random.value).ToList();
            }

            this.currentPlay = this.audioToPlay[index];
            this.audioToPlay.RemoveAt(index);
            if (this.loop)
            {
                this.audioToPlay.Add(this.currentPlay);
            }
        }

        OnTrackSelected?.Invoke(this, index == -1, index, ref this.currentPlay);

        if (this.logInfo)
        {
            Log.Info($"Loading Audio");
        }

        if (this.allowUrl && Uri.TryCreate(this.currentPlay, UriKind.Absolute, out var result))
        {
            var www = new UnityWebRequest(this.currentPlay, "GET");
            var dH = new DownloadHandlerBuffer();
            www.downloadHandler = dH;

            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.responseCode != 200)
            {
                Log.Error($"Failed to retrieve audio {www.responseCode} {www.downloadHandler.text}");
                if (!this.@continue || this.audioToPlay.Count < 1)
                {
                    yield break;
                }

                yield return Timing.WaitForSeconds(1);
                if (this.audioToPlay.Count >= 1)
                {
                    Timing.RunCoroutine(this.Playback(0));
                }

                yield break;
            }

            this._currentPlayStream = new MemoryStream(www.downloadHandler.data);
        }
        else
        {
            if (File.Exists(this.currentPlay))
            {
                if (!this.currentPlay.EndsWith(".ogg"))
                {
                    Log.Error($"Audio file {this.currentPlay} is not valid. Audio files must be ogg files");
                    yield return Timing.WaitForSeconds(1);
                    if (this.audioToPlay.Count >= 1)
                    {
                        Timing.RunCoroutine(this.Playback(0));
                    }

                    yield break;
                }

                this._currentPlayStream = new MemoryStream(File.ReadAllBytes(this.currentPlay));
            }
            else
            {
                Log.Error($"Audio file {this.currentPlay} does not exist. skipping.");
                yield return Timing.WaitForSeconds(1);
                if (this.audioToPlay.Count >= 1)
                {
                    Timing.RunCoroutine(this.Playback(0));
                }

                yield break;
            }
        }

        this._currentPlayStream.Seek(0, SeekOrigin.Begin);

        this.VorbisReader = new VorbisReader(this._currentPlayStream);

        if (this.VorbisReader.Channels >= 2)
        {
            Log.Error($"Audio file {this.currentPlay} is not valid. Audio files must be mono.");
            yield return Timing.WaitForSeconds(1);
            if (this.audioToPlay.Count >= 1)
            {
                Timing.RunCoroutine(this.Playback(0));
            }

            this.VorbisReader.Dispose();
            this._currentPlayStream.Dispose();
            yield break;
        }

        if (this.VorbisReader.SampleRate != 48000)
        {
            Log.Error($"Audio file {this.currentPlay} is not valid. Audio files must have a SamepleRate of 48000");
            yield return Timing.WaitForSeconds(1);
            if (this.audioToPlay.Count >= 1)
            {
                Timing.RunCoroutine(this.Playback(0));
            }

            this.VorbisReader.Dispose();
            this._currentPlayStream.Dispose();
            yield break;
        }

        OnTrackLoaded?.Invoke(this, index == -1, index, this.currentPlay);

        if (this.logInfo)
        {
            Log.Info($"Playing {this.currentPlay} with samplerate of {this.VorbisReader.SampleRate}");
        }

        this.samplesPerSecond = VoiceChatSettings.SampleRate * VoiceChatSettings.Channels;

        //_samplesPerSecond = VorbisReader.Channels * VorbisReader.SampleRate / 5;
        this.SendBuffer = new float[(this.samplesPerSecond / 5) + HeadSamples];
        this.ReadBuffer = new float[(this.samplesPerSecond / 5) + HeadSamples];
        while (this.VorbisReader.ReadSamples(this.ReadBuffer, 0, this.ReadBuffer.Length) > 0)
        {
            if (this.stopTrack)
            {
                this.VorbisReader.SeekTo(this.VorbisReader.TotalSamples - 1);
                this.stopTrack = false;
            }

            while (!this.shouldPlay)
            {
                yield return Timing.WaitForOneFrame;
            }

            while (this.StreamBuffer.Count >= this.ReadBuffer.Length)
            {
                this.ready = true;
                yield return Timing.WaitForOneFrame;
            }

            foreach (var t in this.ReadBuffer)
            {
                this.StreamBuffer.Enqueue(t);
            }
        }

        if (this.logInfo)
        {
            Log.Info($"Track Complete.");
        }

        var nextQueuepos = 0;
        switch (this.@continue)
        {
            case true when this.loop && index == -1:
                nextQueuepos = -1;
                Timing.RunCoroutine(this.Playback(nextQueuepos));
                OnFinishedTrack?.Invoke(this, this.currentPlay, true, ref nextQueuepos);
                yield break;
            case true when this.audioToPlay.Count >= 1:
                this.isFinished = true;
                Timing.RunCoroutine(this.Playback(nextQueuepos));
                OnFinishedTrack?.Invoke(this, this.currentPlay, index == -1, ref nextQueuepos);
                yield break;
        }

        this.isFinished = true;
        OnFinishedTrack?.Invoke(this, this.currentPlay, index == -1, ref nextQueuepos);

        if (this.clearOnFinish)
        {
            Destroy(this);
        }
    }

    public void Update()
    {
        if (this.Owner is null || !this.ready || this.StreamBuffer.Count == 0 || !this.shouldPlay)
        {
            return;
        }

        this.allowedSamples += Time.deltaTime * this.samplesPerSecond;
        var toCopy = Mathf.Min(Mathf.FloorToInt(this.allowedSamples), this.StreamBuffer.Count);
        if (this.logDebug)
        {
            Log.Debug(
                $"1 {toCopy} {this.allowedSamples} {this.samplesPerSecond} {this.StreamBuffer.Count} {this.PlaybackBuffer.Length} {this.PlaybackBuffer.WriteHead}");
        }

        if (toCopy > 0)
        {
            for (var i = 0; i < toCopy; i++)
            {
                this.PlaybackBuffer.Write(this.StreamBuffer.Dequeue() * (this.Volume / 100f));
            }
        }

        if (this.logDebug)
        {
            Log.Debug(
                $"2 {toCopy} {this.allowedSamples} {this.samplesPerSecond} {this.StreamBuffer.Count} {this.PlaybackBuffer.Length} {this.PlaybackBuffer.WriteHead}");
        }

        this.allowedSamples -= toCopy;

        while (this.PlaybackBuffer.Length >= 480)
        {
            this.PlaybackBuffer.ReadTo(this.SendBuffer, (long)480, 0L);
            var dataLen = this.Encoder.Encode(this.SendBuffer, this.EncodedBuffer, 480);

            foreach (var plr in ReferenceHub.AllHubs.Where(
                         plr => plr.connectionToClient != null && PlayerIsConnected(plr) &&
                                this.broadcastTo.Contains(plr.PlayerId)))
            {
                plr.connectionToClient.Send(
                    new VoiceMessage(this.Owner, this.BroadcastChannel, this.EncodedBuffer, dataLen, false));
            }
        }
    }

    /// <summary>
    /// Checks whether a player connected to the server is considered fully connected or if it is a DummyPlayer.
    /// </summary>
    /// <param name="hub">The ReferenceHub of the player to check.</param>
    /// <returns>True if the player is fully connected and not a DummyPlayer; otherwise, false.</returns>
    public static bool PlayerIsConnected(ReferenceHub hub)
    {
        return hub.authManager.InstanceMode == ClientInstanceMode.ReadyClient &&
               hub.nicknameSync.NickSet &&
               !hub.isLocalPlayer &&
               !string.IsNullOrEmpty(hub.authManager.UserId) &&
               !hub.authManager.UserId.Contains("Dummy");
    }
}