﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinGameMenuController : IMenuController {
	private JoinGameMenuModel model;
	private MonoBehaviour mb;	// Kludge for delaying methods
	// DO NOT USE mb FOR ANYTHING OTHER THAN StartCoroutine!!!

	public JoinGameMenuController(JoinGameMenuModel model) {
		this.model = model;
		mb = GameObject.FindObjectOfType<MonoBehaviour>();
	}

	public void action(string identifier) {
		// Remove current menu
		model.terminate();

		// Start the in-game menu
		mb.StartCoroutine(inGameMenu());
	}

	public void init() {
		model.init();
	}

	private IEnumerator inGameMenu() {
		yield return new WaitForSeconds(1f);
		IngameMenuModel model = new IngameMenuModel();
		IngameMenuController controller = new IngameMenuController(model);
		new IngameMenuView(model, controller);
	}

	public void joinGame(NetworkGame game) {
		this.model.terminate();
		Debug.Log("Joining host " + game.hostId);
		this.PostNotification("gameRequest", game.gameType.ToLower());
		this.PostNotification(NetworkController.BEGIN_JOIN_GAME, game.hostId);

		IngameMenuModel model = new IngameMenuModel();
		IngameMenuController controller = new IngameMenuController(model);
		new IngameMenuView(model, controller);
	}
}
