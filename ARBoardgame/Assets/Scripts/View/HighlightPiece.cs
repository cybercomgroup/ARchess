using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightPiece : MonoBehaviour {

	public bool Highlighted = false;
	public string materialName;
	public Material material;
	Material glowMaterial;
	Shader normalShader;
	Color temp;

	private void Awake()
	{

		normalShader = Shader.Find("Standard");
		if (material == null)
		{
			glowMaterial = Resources.Load<Material>("materialName");
		}
		else
		{
			glowMaterial = material;
		}

		if (gameObject.CompareTag("Untagged"))
		{
			gameObject.tag = "HighlightPickup";
		}
	}

	void LateUpdate () {
		if (Highlighted)
		{
			//Just testing, need to be changed to handle shaders later.
			temp = GetComponent<Renderer> ().material.color;
			GetComponent<Renderer> ().material.color = Color.red;
		}
		else
		{
			GetComponent<Renderer> ().material.color = Color.black;
		}
		Highlighted = false;

	}
}