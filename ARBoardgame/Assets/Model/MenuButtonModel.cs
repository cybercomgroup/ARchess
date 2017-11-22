﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonModel {
	public string identifier { get; set; }
	public string text { get; set; }

	public MenuButtonModel(string identifier) : this(identifier, "ChangeMe") {
	}

	public MenuButtonModel(string identifier, string text) {
		this.identifier = identifier;
		this.text = text;
	}

	public void init() {
		this.PostNotification("menuButtonUpdate");
	}
}
