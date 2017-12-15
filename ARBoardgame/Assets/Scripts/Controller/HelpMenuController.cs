using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuController : IMenuController {
	private HelpMenuModel model;
	private MonoBehaviour mb;	// Kludge for delaying methods
	// DO NOT USE mb FOR ANYTHING OTHER THAN StartCoroutine!!!

	public HelpMenuController(HelpMenuModel model) {
		this.model = model;
		mb = GameObject.FindObjectOfType<MonoBehaviour>();
	}

	public void OnBackClicked(object sender, object args)
	{
		mb.StartCoroutine(startMenu());
		model.terminate();
		this.RemoveObserver(OnBackClicked, ArInteractionController.BACK_CLICKED);
	}

	private IEnumerator startMenu()
	{
		yield return new WaitForSeconds(1f);
		MainMenuModel mainMenuModel = new MainMenuModel();
		MainMenuController mainMenuController = new MainMenuController(mainMenuModel);
		new MainMenuView(mainMenuModel, mainMenuController);

		mainMenuController.init();
	}
	
	public void action(string identifier) {
	}

	public void init() {
		model.init();
		this.AddObserver(OnBackClicked, ArInteractionController.BACK_CLICKED);
	}
}
