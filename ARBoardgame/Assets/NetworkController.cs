using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class NetworkController : MonoBehaviour {
    public NetworkManager net;
    private bool busy;

    [SerializeField]
    private uint roomSize = 2;

    private string roomName;

    List<MatchInfoSnapshot> matches;

	// Use this for initialization
	void Start () {
        busy = false;

        //PlayerPrefs.SetString("CloudNetworkingId", "4862202");
        //net.StartMatchMaker();
        //net.matchMaker.SetProgramAppID((AppID)4862202);
        
        if (net.matchMaker == null)
        {
            net.StartMatchMaker();
        }
        matches = null;

        roomName = "MyRoom";
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.H) && !busy)
        {
            busy = true;
            // net.StartHost();
            net.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, net.OnMatchCreate);
        }

        if (Input.GetKeyDown(KeyCode.J) && !busy)
        {
            //net.StartClient();
            if (matches != null && matches.Count > 0)
            {
                busy = true;
                net.matchMaker.JoinMatch(matches[0].networkId, "", "", "", 0, 0, net.OnMatchJoined);
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && !busy)
        {
            RefreshRoomList();
        }
    }

    void RefreshRoomList()
    {
        net.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success)
        {
            this.matches = matches;
        }
    }


    /*void OnDestroy()
    {
        net.matchMaker.DestroyMatch(netId, 0, OnMatchDestroy);
    }



    */

    public void OnMatchDestroy(bool success, string extendedInfo)
    {

    }
}
