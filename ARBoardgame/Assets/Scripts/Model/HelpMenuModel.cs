using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuModel {
	public TextModel heading { get; set; }
	public TextModel names { get; set; }

	public HelpMenuModel() {
		heading = new TextModel("heading", "This game was made by:");
		names = new TextModel("heading", "Dennis Ek, Daniel Felczak, Gustaf Järgren, Magnus Larsson, Filip Törnqvist");
	}

	public void init() {
		heading.init();
		names.init();
	}

	public void terminate() {
		heading.terminate();
		names.terminate();
	}
}
