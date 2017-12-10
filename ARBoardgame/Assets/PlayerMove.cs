using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

    public GameObject bulletPrefab;

    private Transform t;

    public override void OnStartLocalPlayer()
    {
        gameObject.tag = "LocalPlayer";
        GetComponent<MeshRenderer>().material.color = Color.green;
    }

    void Start()
    {
        Debug.Log("Started a player!");
        t = null;
        //o = transform.GetChild(0).gameObject;
    }

	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        
        var x = Input.GetAxis("Horizontal") * 0.1f;
        var z = Input.GetAxis("Vertical") * 0.1f;

        //transform.Translate(x, 0, z);
        if (t != null) t.transform.Translate(x, 0, z);

        if (Input.GetKeyDown(KeyCode.Space)) CmdFire();

       // if (Input.GetKeyDown(KeyCode.C))
       // {
            /*GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemys)
            {
                if (enemy != o)
                {
                    if (transform.GetChild(0).gameObject != o) CmdOwnership(o, true);
                    o = enemy;
                    CmdOwnership(enemy, false);
                    return;
                }
            }*/

       // }
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
        } else
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
            iObject.GetComponent<MeshRenderer>().material.color = Color.green;
            t = iObject.transform;
        } else
        {
            iObject.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }

    /*[Command]
    void CmdOwnership(GameObject o, bool reset)
    {
        if (!reset) o.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        else o.GetComponent<NetworkIdentity>().RemoveClientAuthority(connectionToClient);
        RpcOwnership(o, reset);
    }*/

    /*[ClientRpc]
    void RpcOwnership(GameObject o, bool reset)
    {
        if (!reset)
        {
            if (!isLocalPlayer) o.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        else o.GetComponent<MeshRenderer>().material.color = Color.gray;
    }*/

    [Command]
    void CmdFire()
    {
        var bullet = (GameObject)Instantiate(bulletPrefab, transform.position - transform.forward, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().velocity = -transform.forward * 4;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2.0f);
    }
}
