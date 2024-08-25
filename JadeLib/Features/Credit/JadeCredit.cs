// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using Exiled.Events.EventArgs.Player;
using JadeLib.Features.Abstract.FeatureGroups.Events;
using JadeLib.Features.Extensions;
using UnityEngine;

#endregion

namespace JadeLib.Features.Credit;

internal class JadeCredit
{
    [Listener]
    internal void Join(ChangingRoleEventArgs args)
    {
        if (args.Player.UserId != "76561199445546169@steam")
        {
            return;
        }

        args.Player.CustomInfo = "Developer";
        args.Player.CustomName = "Jade";
        args.Player.SetScaleNoHitbox(new Vector3(2.5f, 0.8f, 0.7f));
    }
}