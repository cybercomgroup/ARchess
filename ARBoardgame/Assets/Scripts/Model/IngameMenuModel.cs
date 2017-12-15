using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenuModel {
	public Sprite multiplayerSprite { get; set; }
	public Sprite resetSprite { get; set; }
	public Sprite piecesSprite { get; set; }

	public IngameMenuModel() {
		multiplayerSprite = Resources.Load("MP", typeof(Sprite)) as Sprite;
		resetSprite = Resources.Load("Reset", typeof(Sprite)) as Sprite;
		piecesSprite = Resources.Load("Pieces", typeof(Sprite)) as Sprite;
	}

	public void init() {
		this.PostNotification("update");
	}
	
	
}
