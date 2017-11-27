using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameMenuController : IMenuController {
	private StartGameMenuModel model;

	public StartGameMenuController(StartGameMenuModel model) {
		this.model = model;
	}

	public void action(string identifier) {
		Debug.Log("Pressed " + identifier + " in the \"start game\" menu");
		this.PostNotification("gameRequest", identifier);

		// Remove current menu
		// TODO Make sure that the current View removes itself from NotificationCenter
		GameObject canvas = GameObject.Find("Canvas");
		foreach(Transform t in canvas.transform) {
			Object.Destroy(t.gameObject);
		}

		// Start the in-game menu
		IngameMenuModel model = new IngameMenuModel();
		IngameMenuController controller = new IngameMenuController(model);
		IngameMenuView view = new IngameMenuView(model, controller);
	}

	public void init() {
		model.init();
	}
}
