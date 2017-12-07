using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : MonoBehaviour {
    public NetworkManager net;
    private bool busy;

	// Use this for initialization
	void Start () {
        busy = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.H) && !busy)
        {
            busy = true;
            net.StartHost();
        }

        if (Input.GetKeyDown(KeyCode.J) && !busy)
        {
            busy = true;
            net.StartClient();
        }
    }
}
