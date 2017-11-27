using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameMenuController : IMenuController {
	private StartGameMenuModel model;

	public StartGameMenuController(StartGameMenuModel model) {
		this.model = model;
	}

	public void action(string identifier) {
		this.PostNotification("gameRequest", Regex.Split(identifier, @"(?<!^)(?=[A-Z])")[0]);

		// Remove current menu
		this.model.terminate();

		// Start the in-game menu
		IngameMenuModel model = new IngameMenuModel();
		IngameMenuController controller = new IngameMenuController(model);
		IngameMenuView view = new IngameMenuView(model, controller);
	}

	public void init() {
		model.init();
	}
}
