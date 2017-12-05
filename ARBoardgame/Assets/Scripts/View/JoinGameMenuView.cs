using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameMenuView {
	private TextView headingView;

	public JoinGameMenuView(JoinGameMenuModel model, JoinGameMenuController controller) {
		GameObject.Find("Panel").AddComponent(typeof(FadeIn));
		headingView = new TextView(model.heading, controller);

		GameObject scrollView = new GameObject("scrollView");
		scrollView.transform.SetParent(GameObject.Find("Panel").transform, false);
		ScrollRect scrollRect = scrollView.AddComponent<ScrollRect>() as ScrollRect;
		scrollRect.horizontal = false;

		GameObject viewport = new GameObject("viewport");
		viewport.transform.SetParent(scrollView.transform, false);
		viewport.AddComponent<Mask>();
		Image maskImage = viewport.AddComponent<Image>();
		maskImage.sprite = Resources.Load("background", typeof(Sprite)) as Sprite;
		maskImage.type = Image.Type.Sliced;

		GameObject content = new GameObject("content");
		content.transform.SetParent(viewport.transform, false);
		VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup>();
		vlg.childControlHeight = false;
		vlg.childControlWidth = false;
		vlg.childAlignment = TextAnchor.UpperCenter;
		vlg.spacing = 5;
		content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

		scrollRect.content = content.GetComponent<RectTransform>();
		scrollRect.viewport = viewport.GetComponent<RectTransform>();

		for(int i=0; i<4; i++) {
			GameObject txtObj = GameObject.Instantiate(Resources.Load("MenuText", typeof(GameObject)), content.transform) as GameObject;
			Text text = txtObj.GetComponent<Text>();
			text.text = "Text " + i;
		}
		GameObject asd = GameObject.Instantiate(Resources.Load("MenuButton", typeof(GameObject)), content.transform) as GameObject;
	}
}
