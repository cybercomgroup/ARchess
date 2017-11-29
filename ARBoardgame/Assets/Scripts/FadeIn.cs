using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Set initial alpha to 0 (full transparency)
		GetComponent<CanvasRenderer>().SetAlpha(0f);
		// During 1 second, fade to alpha 1 (zero transparency)
		if(GetComponent<Graphic>() != null) {
			float fadeTarget = 1f;
			// Special case when we're dealing with disabled buttons
			if(GetComponent<Button>() && !GetComponent<Button>().interactable) {
				fadeTarget = 0.5f;
			}
			GetComponent<Graphic>().CrossFadeAlpha(fadeTarget, 1f, false);
		}
		// Destroy this script after 1 seconds, it has served its purpose by then
		Object.Destroy(this, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
