using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGame {
	public int hostId { get; set; }
	public string hostName { get; set; }
	public string gameType { get; set; } // TODO enum??

	public NetworkGame(int hostId, string hostName, string gameType) {
		this.hostId = hostId;
		this.hostName = hostName;
		this.gameType = gameType;
	}
}
