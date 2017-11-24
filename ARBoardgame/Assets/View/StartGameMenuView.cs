using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameMenuView {
	private TextView headingView;
	private MenuButtonView chessButtonView;
	private MenuButtonView goButtonView;
	private MenuButtonView diplomacyButtonView;
	private MenuButtonView reversiButtonView;

	public StartGameMenuView(StartGameMenuModel model, StartGameMenuController controller, Transform canvasTransform, GameObject menuButtonObject) {
		headingView = new TextView(model.heading, controller);
		chessButtonView = new MenuButtonView(model.chessButton, controller, canvasTransform, menuButtonObject);
		goButtonView = new MenuButtonView(model.goButton, controller, canvasTransform, menuButtonObject);
		diplomacyButtonView = new MenuButtonView(model.diplomacyButton, controller, canvasTransform, menuButtonObject);
		reversiButtonView = new MenuButtonView(model.reversiButton, controller, canvasTransform, menuButtonObject);
	}
}
