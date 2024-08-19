using Exiled.Events.EventArgs.Player;
using JadeLib.Features.API.Reflection.Events;
using JadeLib.Features.Extensions;
using UnityEngine;

namespace JadeLib.Features.Credit;

public class JadeCredit
{
    [Listener]
    public void Join(ChangingRoleEventArgs args)
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