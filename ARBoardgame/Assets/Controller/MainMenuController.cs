using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : IMenuController {
	private MainMenuModel model;

	public MainMenuController(MainMenuModel model) {
		this.model = model;
	}

	public void action(string identifier) {
		// TODO 
		Debug.Log("Recieved action " + identifier);
	}

	public void init() {
		model.init();
	}
}
