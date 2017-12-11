using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PieceController : NetworkBehaviour {
    public void OnGrab()
    {
        var player = GameObject.FindGameObjectWithTag("LocalPlayer");
        var playerID = player.GetComponent<NetworkIdentity>();
        player.GetComponent<PlayerController>().CmdGrab(netId, playerID);
    }
}
