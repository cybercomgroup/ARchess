using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuView {
	private MenuButtonView startGameButtonView;
	private MenuButtonView joinGameButtonView;
	private MenuButtonView helpButtonView;

	public MainMenuView(MainMenuModel model, MainMenuController controller, Transform canvasTransform, GameObject menuButtonObject) {
		startGameButtonView = new MenuButtonView(model.startGameButtonModel, controller, canvasTransform, menuButtonObject);
		joinGameButtonView = new MenuButtonView(model.joinGameButtonModel, controller, canvasTransform, menuButtonObject);
		helpButtonView = new MenuButtonView(model.helpButtonModel, controller, canvasTransform, menuButtonObject);
	}
}
