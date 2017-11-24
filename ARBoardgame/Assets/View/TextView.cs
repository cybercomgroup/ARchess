using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextView {
	private Text text;
	private TextModel model;
	private IMenuController controller;

	public TextView(TextModel model, IMenuController controller) {
		this.model = model;
		// Subscribe to updates from "buttonObject"
		model.AddObserver(onUpdate, "textUpdate");

		this.controller = controller;

		GameObject canvasObject = (GameObject) GameObject.Find("Canvas");
		GameObject textObject = new GameObject(model.identifier);
		textObject.transform.SetParent(canvasObject.transform);
		text = textObject.AddComponent<Text>();
		text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
	}

	public void onUpdate(object sender, object args) {
		text.text = model.text;
	}
}
