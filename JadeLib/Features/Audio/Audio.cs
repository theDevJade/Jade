using System;
using System.Linq;
using CentralAuth;
using Exiled.API.Features;
using PlayerRoles;

namespace JadeLib.Features.Audio;

public class Audio(Npc npc)
{
    public ReferenceHub Hub { get; } = npc.ReferenceHub;

    public Npc Npc { get; } = npc;

    public AudioPlayer Player { get; private set; }

    public void AddPlayer(params Player[] players)
    {
        this.Player.broadcastTo.AddRange(players.Select(e => e.Id));
    }

    /// <summary>
    /// Creates a new NPC with audio-playing capabilities.
    /// </summary>
    /// <param name="name">The name of the NPC.</param>
    /// <param name="role">The role to spawn the npc as.</param>
    /// <param name="id">The ID, do not change if you want to follow VSR.</param>
    /// <param name="userID">User ID.</param>
    /// <returns></returns>
    public static Audio CreateNpc(
        string name,
        RoleTypeId role = RoleTypeId.None,
        int id = 0,
        string userID = PlayerAuthenticationManager.DedicatedId)
    {
        var npc = Npc.Spawn(name, role, id, userID);

        try
        {
            if (userID == PlayerAuthenticationManager.DedicatedId)
            {
                npc.ReferenceHub.authManager.SyncedUserId = userID;
                try
                {
                    npc.ReferenceHub.authManager.InstanceMode = ClientInstanceMode.DedicatedServer;
                }
                catch (Exception e)
                {
                    Log.Debug($"Ignore: {e}");
                }
            }
            else
            {
                npc.ReferenceHub.authManager.UserId = userID == string.Empty ? $"Dummy@localhost" : userID;
            }
        }
        catch (Exception e)
        {
            Log.Debug($"Ignore: {e}");
        }

        var audio = new Audio(npc);
        audio.Player = AudioPlayer.GetOrCreate(npc.ReferenceHub, audio);
        return audio;
    }
}