#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Mirror;

#endregion

namespace JadeLib.Features.PlayerVisibility;

public class PlayerHiderMirror
{
    private static readonly Dictionary<NetworkIdentity, Func<NetworkConnection, bool>> predicates = [];

    /// <summary>
    ///     Initializes a new instance of the <see cref="PlayerHiderMirror" /> class.
    /// </summary>
    /// <param name="identity">The identity of the player you want to hide.</param>
    /// <param name="predicate">
    ///     The predicate for network connections returning true to hide the <paramref name="identity" />
    ///     from.
    /// </param>
    public PlayerHiderMirror(NetworkIdentity identity, Func<NetworkConnection, bool> predicate)
    {
        predicates.Add(identity, predicate);
        foreach (var networkConnectionToClient in NetworkServer.connectionsCopy
                     .Where(e => e.identity.observers.Values.Contains(identity.connectionToClient))
                     .Select(e => e.identity.connectionToServer))
        {
            NetworkServer.HideForConnection(identity, networkConnectionToClient);
        }
    }

    /// <summary>
    ///     Gets a <see cref="ReadOnlyDictionary{TKey,TValue}" /> of NetworkIdentities and their Predicates.
    /// </summary>
    public static ReadOnlyDictionary<NetworkIdentity, Func<NetworkConnection, bool>> Predicates => new(predicates);
}