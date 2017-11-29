using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// During 1 second, fade to alpha 0 (full transparency)
		if(GetComponent<Graphic>() != null) {
			GetComponent<Graphic>().CrossFadeAlpha(0f, 1f, false);
		}
		// Destroy this script after 1 second, it has served its purpose by then
		Object.Destroy(this, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
