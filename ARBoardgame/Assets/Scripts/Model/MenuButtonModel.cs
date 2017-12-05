using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonModel {
	public string identifier { get; set; }
	public string text { get; set; }
	public bool enabled { get; set; }

	public MenuButtonModel(string identifier) : this(identifier, "ChangeMe") {
	}

	public MenuButtonModel(string identifier, string text) {
		this.identifier = identifier;
		this.text = text;
		enabled = true;
	}

	public void init() {
		this.PostNotification("menuButtonUpdate");
	}

	public void terminate() {
		this.PostNotification("menuButtonTerminate");
	}
}
