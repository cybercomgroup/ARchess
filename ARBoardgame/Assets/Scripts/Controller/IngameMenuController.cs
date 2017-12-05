using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenuController : IMenuController {
	public IngameMenuModel model { get; set; }

	public IngameMenuController(IngameMenuModel model) {
		this.model = model;
	}

	public void init() {
		model.init();
	}

	public void action(string identifier) {
		Debug.Log("IngameMenuController::action: " + identifier);
	}
}
