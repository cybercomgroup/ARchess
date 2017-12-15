using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuView {
	public HelpMenuView(HelpMenuModel model, HelpMenuController controller) {
		GameObject.Find("Panel").AddComponent(typeof(FadeIn));
		new TextView(model.heading).setTextSize(11);
		new TextView(model.names).setTextSize(10);
	}
}
