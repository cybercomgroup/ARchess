using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameMenuView {
	public StartGameMenuView(StartGameMenuModel model, StartGameMenuController controller) {
		GameObject.Find("Panel").AddComponent(typeof(FadeIn));
		new TextView(model.heading);
		new MenuButtonView(model.chessButton, controller);
		new MenuButtonView(model.goButton, controller);
		new MenuButtonView(model.diplomacyButton, controller);
		new MenuButtonView(model.reversiButton, controller);
	}
}
