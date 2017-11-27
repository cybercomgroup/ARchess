using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameMenuView {
	private IngameMenuModel model;
	private IMenuController controller;

	public IngameMenuView(IngameMenuModel model, IngameMenuController controller) {
		changeToHorizontalLayout();

		this.model = model;
		this.controller = controller;

		setupButton(model.multiplayerSprite).onClick.AddListener(onMultiplayerClick);
		setupButton(model.resetSprite).onClick.AddListener(onResetClick);
		setupButton(model.piecesSprite).onClick.AddListener(onPiecesClick);
	}

	private void changeToHorizontalLayout() {
		GameObject canvasObject = GameObject.Find("Canvas") as GameObject;
		Object.DestroyImmediate(canvasObject.GetComponent(typeof(VerticalLayoutGroup)));
		HorizontalLayoutGroup layoutGroup = canvasObject.AddComponent<HorizontalLayoutGroup>();
		layoutGroup.childControlWidth = false;
		layoutGroup.childControlHeight = false;
		layoutGroup.childForceExpandWidth = true;
		layoutGroup.childForceExpandHeight = true;
		layoutGroup.childAlignment = TextAnchor.LowerCenter;
	}

	private void onMultiplayerClick() {
		Debug.Log("LMAO STARTING MULTIPLAYER HOSTING");
	}

	private void onResetClick() {
		Debug.Log("LMAO RESETTING GAME");
	}

	private void onPiecesClick() {
		Debug.Log("LMAO CHOOSE SOME PIECES");
	}

	private Button setupButton(Sprite sprite) {
		GameObject canvasObject = GameObject.Find("Canvas");
		Transform canvasTransform = canvasObject.GetComponent(typeof(Transform)) as Transform;

		GameObject buttonObject = new GameObject();
		buttonObject.transform.SetParent(canvasTransform);
		Button button = buttonObject.AddComponent<Button>();
		Image image = buttonObject.AddComponent<Image>();
		image.sprite = sprite;
		button.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

		return button;
	}
}
