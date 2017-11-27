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
		// Subscribe to updates from "text"
		model.AddObserver(onUpdate, "textUpdate");
		model.AddObserver(onTerminate, "textTerminate");

		this.controller = controller;

		Transform canvasTransform = GameObject.Find("Canvas").transform;
		GameObject textObject = GameObject.Instantiate(Resources.Load("MenuText", typeof(GameObject))) as GameObject;
		text = textObject.GetComponent(typeof(Text)) as Text;
		textObject.transform.SetParent(canvasTransform);
		textObject.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void onUpdate(object sender, object args) {
		text.text = model.text;
	}

	public void onTerminate(object sender, object args) {
		model.RemoveObserver(onUpdate, "textUpdate");
		model.RemoveObserver(onTerminate, "textTerminate");
		Object.Destroy(text.gameObject);
	}
}
