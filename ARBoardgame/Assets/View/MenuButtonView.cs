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

		Transform canvasTransform = GameObject.Find("Canvas").transform;
		GameObject buttonObject = (GameObject) GameObject.Instantiate(Resources.Load("MenuButton"), canvasTransform);

		// Add click listener for the button
		this.button = (Button) buttonObject.GetComponentInChildren(typeof(Button));
		this.button.onClick.AddListener(onClick);

		// Fade in from transparency
		/*buttonObject.GetComponent<CanvasRenderer>().SetAlpha(0f);
		buttonObject.GetComponent<Image>().CrossFadeAlpha(1f, 1f, false);*/
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
		/*button.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
		Object.Destroy(button.gameObject, 1f);*/
		Object.Destroy(button.gameObject);
	}
}
