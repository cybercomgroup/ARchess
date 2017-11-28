using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightView : MonoBehaviour {

	public bool Highlighted = false;
	public string materialName;
	Material glowMaterial;
	Shader normalShader;
	
	private void Awake()
	{
		normalShader = Shader.Find("Standard");
		glowMaterial = Resources.Load<Material>(materialName);
		gameObject.tag = "Highlightable";
	}
	
	void LateUpdate () {
		if (Highlighted)
		{
			GetComponent<Renderer>().material = glowMaterial;
		}
		else
		{

			GetComponent<Renderer>().material.shader = normalShader;
		}
		Highlighted = false;
	}
}
