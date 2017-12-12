
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class NetworkController : NetworkManager {
    // Use this for initialization


    private List<MatchInfoSnapshot> _matches;
    public string roomName;
    public uint roomSize;

    public static string BEGIN_HOST = "BEGIN HOST";
    public static string BEGIN_FIND_GAMES = "BEGIN FIND GAMES";
    public static string BEGIN_JOIN_GAME = "BEGIN JOIN GAME"; // TAKES ID OF MATCH TO JOIN
    
	void Start () {
        if (base.matchMaker == null)
        {
            base.StartMatchMaker();
        }

        _matches = null;
        roomName = "MyRoom";
        roomSize = 2;
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnEnable()
    {
        this.AddObserver(HostGame, BEGIN_HOST);
        this.AddObserver(JoinGame, BEGIN_FIND_GAMES);
        this.AddObserver(FindGames, BEGIN_JOIN_GAME);
    }

    void OnDisable()
    {
        this.RemoveObserver(HostGame, BEGIN_HOST);
        this.RemoveObserver(JoinGame, BEGIN_FIND_GAMES);
        this.RemoveObserver(FindGames, BEGIN_JOIN_GAME);
    }

    public void HostGame(object sender, object args)
    {
        base.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, base.OnMatchCreate);
    }

    public void JoinGame(object sender, object args)
    {
        int id = (int)args;
        if (_matches != null && _matches.Count > 0)
        {
            base.matchMaker.JoinMatch(_matches[id].networkId, "", "", "", 0, 0, base.OnMatchJoined);
        }
    }

    public void FindGames(object sender, object args)
    {
        base.matchMaker.ListMatches(0, 20, "", false, 0, 0, FoundGames);
    }

    public void FoundGames(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success)
        {
            _matches = matches;

            for (int i = 0; i < matches.Count; i++)
            {
                this.PostNotification(JoinGameMenuView.ADD_NETWORK_GAME_TO_LIST, new NetworkGame(i, "ExampleName", "Chess"));
            }
        }
    }


}
