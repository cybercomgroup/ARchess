using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Combat : NetworkBehaviour {
    public const int maxHealth = 100;
    public bool destroyOnDeath;

    [SyncVar]
    public int health = maxHealth;

    public void TakeDamage(int amount)
    {
        if (!isServer) return;

        health -= amount;
        if (health <= 0)
        {
            if (destroyOnDeath) {
                Destroy(gameObject);
            } else {
                health = maxHealth;
                RpcRespawn();
            }
        }
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            transform.position = Vector3.zero;
        }
    }

    public void OnMouseDown()
    {
        var player = GameObject.FindGameObjectWithTag("LocalPlayer");
        var playerID = player.GetComponent<NetworkIdentity>();
        player.GetComponent<PlayerMove>().CmdGrab(netId, playerID);
    }
}
