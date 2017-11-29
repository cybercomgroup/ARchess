using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : IMenuController {
	private MainMenuModel model;
	private MonoBehaviour mb;	// Kludge for delaying methods
	// DO NOT USE mb FOR ANYTHING OTHER THAN StartCoroutine!!!

	public MainMenuController(MainMenuModel model) {
		this.model = model;
		mb = GameObject.FindObjectOfType<MonoBehaviour>();
		
	}

	public void action(string identifier) {
		// Remove current menu
		model.terminate();

		if(identifier == "startGameButton") {
			mb.StartCoroutine(startGameMenu());
		} else if(identifier == "joinGameButton") {
			// TODO
		} else if(identifier == "helpButton") {
			// TODO
		}
	}

	public void init() {
		model.init();
	}

	private IEnumerator startGameMenu() {
		yield return new WaitForSeconds(1f);
		StartGameMenuModel model = new StartGameMenuModel();
		StartGameMenuController controller = new StartGameMenuController(model);
		StartGameMenuView view = new StartGameMenuView(model, controller);
		
		controller.init();
	}
}
