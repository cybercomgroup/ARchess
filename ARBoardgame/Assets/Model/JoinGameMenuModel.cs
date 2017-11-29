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
	}

	public void terminate() {
		heading.terminate();
	}
}
