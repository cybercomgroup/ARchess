using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightView : MonoBehaviour {

	public bool Highlighted = false;
	public string materialName;
	public Material material;
	Material glowMaterial;
	Shader normalShader;
	
	private void Awake()
	{
		normalShader = Shader.Find("Standard");
		if (material == null)
		{
			glowMaterial = Resources.Load<Material>(materialName);
		}
		else
		{
			glowMaterial = material;
		}
		
		if (gameObject.CompareTag("Untagged"))
		{
			gameObject.tag = "Highlightable";
		}
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
