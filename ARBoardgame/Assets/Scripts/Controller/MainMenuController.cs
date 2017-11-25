using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : IMenuController {
	private MainMenuModel model;

	public MainMenuController(MainMenuModel model) {
		this.model = model;
	}

	public void action(string identifier) {
		// Remove current menu
		// TODO Make sure that the current View removes itself from NotificationCenter
		GameObject canvas = GameObject.Find("Canvas");
		foreach(Transform t in canvas.transform) {
			Object.Destroy(t.gameObject);
		}

		// Prepare for the new menu
		Transform canvasTransform = (Transform) GameObject.Find("Canvas").GetComponent(typeof(Transform));
		GameObject menuButtonObject = Menu.fixme;

		if(identifier == "startGameButton") {
			StartGameMenuModel model = new StartGameMenuModel();
			StartGameMenuController controller = new StartGameMenuController(model);
			StartGameMenuView view = new StartGameMenuView(model, controller, canvasTransform, menuButtonObject);
			
			controller.init();
		} else if(identifier == "joinGameButton") {
			// TODO
		} else if(identifier == "helpButton") {
			// TODO
		}
	}

	public void init() {
		model.init();
	}
}
