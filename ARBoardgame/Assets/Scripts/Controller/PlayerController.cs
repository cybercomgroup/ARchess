using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public const string started = "PlayerController.started";
    public const string startedLocal = "PlayerController.startedLocal";
    public const string destroyed = "PlayerController.destroyed"; // player left the game / crashed
    public const string requestPickup = "PlayerController.requestPickup";

    public bool pickupActive; // true if this player is currently carrying a piece.

    public enum PlayerColor { white, black, none };

    public override void OnStartClient()
    {
        base.OnStartClient();
        this.PostNotification(started);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        this.PostNotification(startedLocal);
    }

    void OnDestroy()
    {
        this.PostNotification(destroyed);
    }

    [Command]
    public void CmdRequestPickup(PlayerColor c, int piece)
    {
        if (c == PlayerColor.none) return; // ignore
        if (true /*c == activeColor*/) RpcPickup(c, piece);
    }

    [ClientRpc]
    void RpcPickup(PlayerColor c, int piece)
    {

    }
}
