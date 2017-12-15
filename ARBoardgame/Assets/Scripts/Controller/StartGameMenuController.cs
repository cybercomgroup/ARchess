using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class StartGameMenuController : IMenuController {
	private StartGameMenuModel model;
	private MonoBehaviour mb;	// Kludge for delaying methods
	// DO NOT USE mb FOR ANYTHING OTHER THAN StartCoroutine!!!

	public StartGameMenuController(StartGameMenuModel model) {
		this.model = model;
		mb = GameObject.FindObjectOfType<MonoBehaviour>();
	}
	public void OnBackClicked(object sender, object args)
	{
		mb.StartCoroutine(startMenu());
		model.terminate();
		this.RemoveObserver(OnBackClicked, ArInteractionController.BACK_CLICKED);
	}
	
	public void action(string identifier) {
		// Notice the app about a game start request
		this.PostNotification("gameRequest", Regex.Split(identifier, @"(?<!^)(?=[A-Z])")[0]);

		// Remove current menu
		model.terminate();

		// Start the in-game menu
		mb.StartCoroutine(inGameMenu());
	}

	public void init() {
		model.init();
		this.AddObserver(OnBackClicked, ArInteractionController.BACK_CLICKED);
	}

	private IEnumerator startMenu()
	{
		yield return new WaitForSeconds(1f);
		MainMenuModel mainMenuModel = new MainMenuModel();
		MainMenuController mainMenuController = new MainMenuController(mainMenuModel);
		new MainMenuView(mainMenuModel, mainMenuController);

		mainMenuController.init();
	}
	
	private IEnumerator inGameMenu() {
		yield return new WaitForSeconds(1f);
		IngameMenuModel model = new IngameMenuModel();
		IngameMenuController controller = new IngameMenuController(model);
		new IngameMenuView(model, controller);
	}
}
