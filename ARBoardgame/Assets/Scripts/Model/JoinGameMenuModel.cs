using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinGameMenuModel {
	public TextModel heading { get; set; }

	public JoinGameMenuModel() {
		heading = new TextModel("heading", "Join a game");
	}

	public void init() {
		heading.init();
		// TODO Fetch active games, display them
		this.AddObserver(onNetworkGameDiscovered, "networkGameDiscovered");
	}

	public void terminate() {
		this.PostNotification("joinGameMenuTerminate");
		this.RemoveObserver(onNetworkGameDiscovered, "networkGameDiscovered");
		heading.terminate();
	}

	public void onNetworkGameDiscovered(object sender, object args) {
		this.PostNotification("addNetworkGameToList", args);
	}
}
