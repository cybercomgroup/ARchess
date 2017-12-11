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

		material = GetComponent<Renderer> ().material;
		temp = material.color;

		if (gameObject.CompareTag("Untagged"))
		{
			gameObject.tag = "HighlightPickup";
		}
	}

	void LateUpdate () {
		if (Highlighted)
		{
			material.color = Color.red;
		}
		else
		{
			GetComponent<Renderer> ().material.color = temp;
		}
		Highlighted = false;

	}
}