using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    private GameObject picked;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        gameObject.tag = "LocalPlayer";
    }

    void Start()
    {
        picked = null;
    }

    [Command]
    public void CmdGrab(NetworkInstanceId objectId, NetworkIdentity player)
    {
        Debug.Log("CmdGrab called!");
        var iObject = NetworkServer.FindLocalObject(objectId);
        var networkIdentity = iObject.GetComponent<NetworkIdentity>();
        var otherOwner = networkIdentity.clientAuthorityOwner;

        if (otherOwner == player.connectionToClient)
        {
            Debug.Log("CmdGrab: Already has authority!");
            return;
        }
        else
        {
            if (otherOwner != null)
            {
                networkIdentity.RemoveClientAuthority(otherOwner);
            }
            networkIdentity.AssignClientAuthority(player.connectionToClient);
            RpcGrab(objectId);
        }
        Debug.Log("CmdGrab finished!");
    }

    [ClientRpc]
    void RpcGrab(NetworkInstanceId objectId)
    {
        Debug.Log("RpcGrab called!");
        var iObject = ClientScene.FindLocalObject(objectId);
        if (isLocalPlayer)
        {
            picked = iObject; // called on client that owns this player
            // I am now free to edit this object.
        }
        else
        {
            if (picked == iObject) picked = null; // called on client that doesnt own this player.
        }
    }
}
