using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameMenuView {
	private TextView headingView;
	private MenuButtonView chessButtonView;
	private MenuButtonView goButtonView;
	private MenuButtonView diplomacyButtonView;
	private MenuButtonView reversiButtonView;

	public StartGameMenuView(StartGameMenuModel model, StartGameMenuController controller) {
		GameObject.Find("Panel").AddComponent(typeof(FadeIn));
		headingView = new TextView(model.heading, controller);
		chessButtonView = new MenuButtonView(model.chessButton, controller);
		goButtonView = new MenuButtonView(model.goButton, controller);
		diplomacyButtonView = new MenuButtonView(model.diplomacyButton, controller);
		reversiButtonView = new MenuButtonView(model.reversiButton, controller);
	}
}
