
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class NetworkController : NetworkManager {
    // Use this for initialization


    private List<MatchInfoSnapshot> matches;
    public string roomName;
    public uint roomSize;

	void Start () {
        if (base.matchMaker == null)
        {
            base.StartMatchMaker();
        }

        matches = null;
        roomName = "MyRoom";
        roomSize = 2;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void HostGame()
    {
        base.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, base.OnMatchCreate);
    }

    public void JoinGame(int id)
    {
        if (matches != null && matches.Count > 0)
        {
            base.matchMaker.JoinMatch(matches[id].networkId, "", "", "", 0, 0, base.OnMatchJoined);
        }
    }

    public void FindGames()
    {
        base.matchMaker.ListMatches(0, 20, "", false, 0, 0, FoundGames);
    }

    public void FoundGames(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success)
        {
            this.matches = matches;
        }
    }


}
