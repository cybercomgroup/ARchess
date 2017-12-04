using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : NetworkManager {
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    void StartAsHost()
    {
        StartHost();
    }

    void StartAsClient()
    {
        StartClient(); // missing ip, default localhost?
    }
}
