using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameMenuController : IMenuController {
	private StartGameMenuModel model;

	public StartGameMenuController(StartGameMenuModel model) {
		this.model = model;
	}

	public void action(string identifier) {
		Debug.Log("Pressed " + identifier + " in the \"start game\" menu");
	}

	public void init() {
		model.init();
	}
}
