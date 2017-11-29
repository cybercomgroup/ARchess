using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonView {
	private Button button;
	private MenuButtonModel model;
	private IMenuController controller;

	public MenuButtonView(MenuButtonModel model, IMenuController controller) {
		// Subscribe to updates from the button model
		this.model = model;
		model.AddObserver(onUpdate, "menuButtonUpdate");
		model.AddObserver(onTerminate, "menuButtonTerminate");

		this.controller = controller;

		Transform panelTransform = GameObject.Find("Panel").transform;
		GameObject buttonObject = (GameObject) GameObject.Instantiate(Resources.Load("MenuButton"), panelTransform);
		buttonObject.name = model.identifier;
		buttonObject.AddComponent(typeof(FadeIn));
		foreach(Transform child in buttonObject.transform) {
			child.gameObject.AddComponent(typeof(FadeIn));
		}

		// Add click listener for the button
		this.button = (Button) buttonObject.GetComponentInChildren(typeof(Button));
		this.button.onClick.AddListener(onClick);
	}

	public void onClick() {
		controller.action(model.identifier);
	}

	public void onUpdate(object sender, object args) {
		((Text) button.GetComponentInChildren(typeof(Text))).text = model.text;
		button.interactable = model.enabled;
	}

	public void onTerminate(object sender, object args) {
		model.RemoveObserver(onUpdate, "menuButtonUpdate");
		model.RemoveObserver(onTerminate, "menuButtonTerminate");
		button.gameObject.AddComponent(typeof(FadeOut));
		foreach(Transform child in button.gameObject.transform) {
			child.gameObject.AddComponent(typeof(FadeOut));
		}
		Object.Destroy(button.gameObject, 1f);
	}
}
