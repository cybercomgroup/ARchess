using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuModel {
	public MenuButtonModel startGameButtonModel { get; set; }
	public MenuButtonModel joinGameButtonModel { get; set; }
	public MenuButtonModel helpButtonModel { get; set; }

	public MainMenuModel() {
		startGameButtonModel = new MenuButtonModel("startGameButton", "Start game");
		joinGameButtonModel = new MenuButtonModel("joinGameButton", "Join game");
		helpButtonModel = new MenuButtonModel("helpButton", "Help");
	}

	public void init() {
		startGameButtonModel.init();
		joinGameButtonModel.init();
		helpButtonModel.init();
	}

	public void terminate() {
		startGameButtonModel.terminate();
		joinGameButtonModel.terminate();
		helpButtonModel.terminate();
	}
}
