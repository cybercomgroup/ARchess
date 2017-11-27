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
		model.terminate();

		if(identifier == "startGameButton") {
			StartGameMenuModel model = new StartGameMenuModel();
			StartGameMenuController controller = new StartGameMenuController(model);
			StartGameMenuView view = new StartGameMenuView(model, controller);
			
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
