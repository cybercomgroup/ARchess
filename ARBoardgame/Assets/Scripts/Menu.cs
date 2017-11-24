using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
	public GameObject menuButtonObject;

	// Use this for initialization
	void Start () {
		MainMenuModel mainMenuModel = new MainMenuModel();
		MainMenuController mainMenuController = new MainMenuController(mainMenuModel);
		MainMenuView mainMenuView = new MainMenuView(mainMenuModel, mainMenuController, transform, menuButtonObject);

		mainMenuController.init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}