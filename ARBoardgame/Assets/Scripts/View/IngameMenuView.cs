using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameMenuView
{
    private IngameMenuModel model;
    private IMenuController controller;

    public IngameMenuView(IngameMenuModel model, IngameMenuController controller)
    {
        GameObject panelGO = GameObject.Find("Panel");
        if (panelGO != null)
        {
            panelGO.SetActive(false);
        }

        changeToHorizontalLayout();

        Button button = setupButton(model.multiplayerSprite);
        if(button != null)
        {
            button.onClick.AddListener(onMultiplayerClick);
        }

        button = setupButton(model.resetSprite);
        if (button != null)
        {
             button.onClick.AddListener(onResetClick);
        }
        
        button = setupButton(model.piecesSprite);

        if (button != null)
        {
            button.onClick.AddListener(onPiecesClick);
        }
    }

    private void changeToHorizontalLayout()
    {
        GameObject canvasObject = GameObject.Find("Canvas") as GameObject;
        if (canvasObject != null)
        {
            //Object.DestroyImmediate(canvasObject.GetComponent(typeof(VerticalLayoutGroup)));
            HorizontalLayoutGroup layoutGroup = canvasObject.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.childControlWidth = false;
            layoutGroup.childControlHeight = false;
            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childForceExpandHeight = true;
            layoutGroup.childAlignment = TextAnchor.LowerCenter;
        }
    }

    private void onMultiplayerClick()
    {
        Debug.Log("LMAO STARTING MULTIPLAYER HOSTING");
    }

    private void onResetClick()
    {
        Debug.Log("LMAO RESETTING GAME");
    }

    private void onPiecesClick()
    {
        Debug.Log("LMAO CHOOSE SOME PIECES");
    }

    private Button setupButton(Sprite sprite)
    {
        GameObject canvasObject = GameObject.Find("Canvas");
        if (canvasObject != null)
        {
            Transform canvasTransform = canvasObject.GetComponent(typeof(Transform)) as Transform;

            GameObject buttonObject = new GameObject();
            buttonObject.transform.SetParent(canvasTransform);
            Button button = buttonObject.AddComponent<Button>();
            Image image = buttonObject.AddComponent<Image>();
            image.sprite = sprite;
            button.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            return button;
        }
        return null;
    }
}
