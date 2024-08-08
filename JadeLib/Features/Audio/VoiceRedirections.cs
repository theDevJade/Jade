// <copyright file="VoiceRedirections.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using JadeLib.Features.API.Reflection;
using JadeLib.Features.API.Reflection.Events;
using VoiceChat;
using VoiceChat.Networking;

namespace JadeLib.Features;

/// <summary>
/// Voice Redirections to be used with voice chatting event.
/// </summary>
public sealed class VoiceRedirections
{
    /// <summary>
    /// Gets the default voice redirections.
    /// <remarks>Can be null if not initialized.</remarks>
    /// </summary>
    public static VoiceRedirections Default { get; private set; }

    /// <summary>
    /// A list of all voice redirections.
    /// </summary>
    public Dictionary<Func<Player, bool>, Tuple<Func<Player, bool>, VoiceChatChannel>> Predicates { get; private set; } = new();

    /// <summary>
    /// Add a redirection.
    /// </summary>
    /// <param name="inputPredicate">An input predicate for players in the VoiceChatting event.</param>
    /// <param name="outputPredicate">A predicate to be applied to Player.List to send the message to.</param>
    /// <param name="channel">The channel (None to use current).</param>
    public void AddRedirection(Func<Player, bool> inputPredicate, Func<Player, bool> outputPredicate, VoiceChatChannel channel = VoiceChatChannel.None) =>
        this.Predicates.Add(inputPredicate, new Tuple<Func<Player, bool>, VoiceChatChannel>(outputPredicate, channel));

    /// <summary>
    /// Remove a redirection from the redirections.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>A value indicating whether the key was removed or not.</returns>
    public bool RemoveRedirection(Func<Player, bool> key) => this.Predicates.Remove(key);

    internal static void Init()
    {
        Default ??= new VoiceRedirections();
        var featureGroup = new FeatureGroup("voice_redirections");
        featureGroup.Supply(Default);
        featureGroup.Register();
    }

    [Listener]
    private void OnVoiceChatting(VoiceChattingEventArgs args)
    {
        var validPredicates = this.Predicates.Keys.ToList().Where(e => e(args.Player));
        foreach (var pair in this.Predicates.Where(pair => validPredicates.Contains(pair.Key)))
        {
            args.IsAllowed = false;
            var targetChannel =
                pair.Value.Item2 == VoiceChatChannel.None ? args.VoiceMessage.Channel : pair.Value.Item2;
            foreach (var player in Player.List.Where(e => pair.Value.Item1(e)))
            {
                player.ReferenceHub.connectionToClient.Send(new VoiceMessage(args.Player.ReferenceHub, targetChannel, args.VoiceMessage.Data, args.VoiceMessage.DataLength, args.VoiceMessage.SpeakerNull));
            }
        }
    }
}