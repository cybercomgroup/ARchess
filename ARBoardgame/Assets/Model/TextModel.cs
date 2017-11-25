using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextModel {
	public string identifier { get; set; }
	public string text { get; set; }

	public TextModel(string identifier) : this(identifier, "ChangeMe") {
	}

	public TextModel(string identifier, string text) {
		this.identifier = identifier;
		this.text = text;
	}

	public void init() {
		this.PostNotification("textUpdate");
	}
}
