using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

    public GameObject bulletPrefab;
	
    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;

        var x = Input.GetAxis("Horizontal") * 0.1f;
        var z = Input.GetAxis("Vertical") * 0.1f;

        transform.Translate(x, 0, z);

        var x2 = Input.GetAxis("Horizontal") * 0.2f;
        var z2 = Input.GetAxis("Vertical") * 0.2f;

        transform.GetChild(0).Translate(x2, 0, z2);


        if (Input.GetKeyDown(KeyCode.Space)) CmdFire();
	}

    [Command]
    void CmdFire()
    {
        var bullet = (GameObject)Instantiate(bulletPrefab, transform.position - transform.forward, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().velocity = -transform.forward * 4;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2.0f);
    }
}
