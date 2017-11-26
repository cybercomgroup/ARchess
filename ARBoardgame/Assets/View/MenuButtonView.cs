using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonView {
	private Button button;
	private MenuButtonModel model;
	private IMenuController controller;

	public MenuButtonView(MenuButtonModel model, IMenuController controller, Transform canvasTransform, GameObject menuButtonObject) {
		this.model = model;
		// Subscribe to updates from "buttonObject"
		model.AddObserver(onUpdate, "menuButtonUpdate");

		this.controller = controller;

		GameObject buttonObject = (GameObject) GameObject.Instantiate(menuButtonObject, canvasTransform);

		this.button = (Button) buttonObject.GetComponentInChildren(typeof(Button));
		// Add click listener for the button
		this.button.onClick.AddListener(onClick);
	}

	public void onClick() {
		controller.action(model.identifier);
	}

	public void onUpdate(object sender, object args) {
		((Text) button.GetComponentInChildren(typeof(Text))).text = model.text;
		button.interactable = model.enabled;
	}
}
