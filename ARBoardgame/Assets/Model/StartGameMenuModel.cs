using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameMenuModel {
	public TextModel heading { get; set; }
	public MenuButtonModel chessButton { get; set; }
	public MenuButtonModel goButton { get; set; }
	public MenuButtonModel diplomacyButton { get; set; }
	public MenuButtonModel reversiButton { get; set; }

	public StartGameMenuModel() {
		heading = new TextModel("heading", "Select a game");
		chessButton = new MenuButtonModel("chessButton", "Chess");
		goButton = new MenuButtonModel("goButton", "Go");
		goButton.enabled = false;
		diplomacyButton = new MenuButtonModel("diplomacyButton", "Diplomacy");
		diplomacyButton.enabled = false;
		reversiButton = new MenuButtonModel("reversiButton", "Reversi");
		reversiButton.enabled = false;
	}

	public void init() {
		heading.init();
		chessButton.init();
		goButton.init();
		diplomacyButton.init();
		reversiButton.init();
	}
}
