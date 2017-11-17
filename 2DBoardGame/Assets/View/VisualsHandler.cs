using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualsHandler
{
    private Dictionary<string, Sprite> gameItemTypeToSpriteMap;
    private Dictionary<string, GameObject> gameItemToGameObjectMap;

    public VisualsHandler()
    {
        gameItemTypeToSpriteMap = new Dictionary<string, Sprite>();
        gameItemToGameObjectMap = new Dictionary<string, GameObject>();
    }

    public void RegisterGameItemTypeWithSprite(string gameItemType, Sprite sprite)
    {
        gameItemTypeToSpriteMap[gameItemType] = sprite;
    }

    public void OnBoardCreated()
    {

    }
}