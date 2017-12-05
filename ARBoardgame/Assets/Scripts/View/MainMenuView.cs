using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuView {
	public MainMenuView(MainMenuModel model, MainMenuController controller) {
		GameObject.Find("Panel").AddComponent(typeof(FadeIn));
		new MenuButtonView(model.startGameButtonModel, controller);
		new MenuButtonView(model.joinGameButtonModel, controller);
		new MenuButtonView(model.helpButtonModel, controller);
	}
}
