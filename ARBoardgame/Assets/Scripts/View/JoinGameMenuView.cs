﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameMenuView {
	private GameObject content;
	private JoinGameMenuController controller;
    public static string ADD_NETWORK_GAME_TO_LIST = "addNetworkGameToList";

	public JoinGameMenuView(JoinGameMenuModel model, JoinGameMenuController controller) {
		GameObject.Find("Panel").AddComponent(typeof(FadeIn));
		new TextView(model.heading);
		this.controller = controller;
		model.AddObserver(terminate, "joinGameMenuTerminate");

		model.AddObserver(addNetworkGameToList, ADD_NETWORK_GAME_TO_LIST);

		createScrollView();

		this.PostNotification(NetworkController.BEGIN_FIND_GAMES);
	}

	public void createScrollView() {
		GameObject scrollView = new GameObject("scrollView");
		scrollView.transform.SetParent(GameObject.Find("Panel").transform, false);
		ScrollRect scrollRect = scrollView.AddComponent<ScrollRect>() as ScrollRect;
		scrollRect.horizontal = false;

		GameObject viewport = new GameObject("viewport");
		viewport.transform.SetParent(scrollView.transform, false);
		viewport.AddComponent<Mask>();
		Image maskImage = viewport.AddComponent<Image>();
		maskImage.sprite = Resources.Load("background", typeof(Sprite)) as Sprite;
		maskImage.type = Image.Type.Sliced;
		viewport.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 100);

		content = new GameObject("content");
		content.transform.SetParent(viewport.transform, false);
		VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup>();
		vlg.childControlHeight = false;
		vlg.childControlWidth = false;
		vlg.childAlignment = TextAnchor.UpperCenter;
		vlg.spacing = 5;
		content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

		scrollRect.content = content.GetComponent<RectTransform>();
		scrollRect.viewport = viewport.GetComponent<RectTransform>();
	}

	public void addNetworkGameToList(object sender, object args) {
		// Create a button in the scroll view
		NetworkGame networkGame = (NetworkGame) args;
		GameObject btnObj = GameObject.Instantiate(Resources.Load("MenuButton", typeof(GameObject)), content.transform) as GameObject;
		btnObj.GetComponentInChildren<Text>().text = networkGame.hostName + " | " + networkGame.gameType;

		// Register button listener for the games
		btnObj.GetComponent<Button>().onClick.AddListener(delegate{controller.joinGame(networkGame);});
	}

	public void terminate(object sender, object args) {
		this.RemoveObserver(terminate, "joinGameMenuTerminate");
		this.RemoveObserver(addNetworkGameToList, ADD_NETWORK_GAME_TO_LIST);
		Object.Destroy(GameObject.Find("Panel"));
	}
}
