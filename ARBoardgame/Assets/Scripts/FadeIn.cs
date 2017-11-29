﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Set initial alpha to 0 (full transparency)
		GetComponent<CanvasRenderer>().SetAlpha(0f);
		// Schedule the fade-in (mostly to let the previous menu fade out)
		Invoke("fadeIn", 1);
		// Destroy this script after 1 seconds, it has served its purpose by then
		Object.Destroy(this, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void fadeIn() {
		// During 1 second, fade to alpha 1 (zero transparency)
		if(GetComponent<Graphic>() != null) {
			GetComponent<Graphic>().CrossFadeAlpha(1f, 1f, false);
		}
	}
}
