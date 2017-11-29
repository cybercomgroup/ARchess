using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuView {
	private MenuButtonView startGameButtonView;
	private MenuButtonView joinGameButtonView;
	private MenuButtonView helpButtonView;

	public MainMenuView(MainMenuModel model, MainMenuController controller) {
		GameObject.Find("Panel").AddComponent(typeof(FadeIn));
		startGameButtonView = new MenuButtonView(model.startGameButtonModel, controller);
		joinGameButtonView = new MenuButtonView(model.joinGameButtonModel, controller);
		helpButtonView = new MenuButtonView(model.helpButtonModel, controller);
	}
}
