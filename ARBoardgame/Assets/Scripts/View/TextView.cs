using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextView {
	private Text text;
	private TextModel model;

	private int textSize = 0;

	public TextView(TextModel model) {
		this.model = model;
		// Subscribe to updates from "text"
		model.AddObserver(onUpdate, "textUpdate");
		model.AddObserver(onTerminate, "textTerminate");

		Transform panelTransform = GameObject.Find("Panel").transform;
		GameObject textObject = GameObject.Instantiate(Resources.Load("MenuText", typeof(GameObject)), panelTransform) as GameObject;
		text = textObject.GetComponent(typeof(Text)) as Text;
		if (textSize != 0)
		{
			text.fontSize = textSize;
		}
		textObject.AddComponent(typeof(FadeIn));
	}

	public void setTextSize(int size)
	{
		textSize = size;
	}

	public void onUpdate(object sender, object args) {
		text.text = model.text;
	}

	public void onTerminate(object sender, object args) {
		model.RemoveObserver(onUpdate, "textUpdate");
		model.RemoveObserver(onTerminate, "textTerminate");
		text.gameObject.AddComponent(typeof(FadeOut));
		Object.Destroy(text.gameObject, 1f);
	}
}
