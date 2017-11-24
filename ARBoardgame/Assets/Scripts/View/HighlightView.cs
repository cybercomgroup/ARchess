using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightView : MonoBehaviour {

	public bool Highlighted = false;
	public string materialName;
	Material glowMaterial;
	Material normalMaterial;
	
	private void Awake()
	{
		normalMaterial = GetComponent<Renderer>().material;
		glowMaterial = Resources.Load<Material>(materialName);
		gameObject.tag = "Highlightable";
	}
	
	void LateUpdate () {
		if (Highlighted == true)
		{
			GetComponent<Renderer>().material = glowMaterial;
		}
		else
		{

			GetComponent<Renderer>().material = normalMaterial;
		}
		Highlighted = false;
	}
}
