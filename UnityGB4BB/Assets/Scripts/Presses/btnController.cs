using UnityEngine;
using System.Collections;

// this controls the UI of the button
public class btnController : MonoBehaviour {

	float minAlpha = 0.25f;
	float alpha = 0.25f;
	bool press = false;

	void onClickGUI(string button, bool isPressed) {
		press = isPressed;
		if (button == this.name)
			if (isPressed == true) {
				alpha = 0.50f;
				Color tCol = this.guiTexture.color;
				tCol.a = alpha;
				this.guiTexture.color = tCol;
			}
	}

	void Awake () {
		pressButton.Instance.onClickGUI += onClickGUI;
	}

	void Start () {
		Debug.Log (Screen.width.ToString() + "." + Screen.height.ToString());
		if(Screen.width == 720) {
			if(Screen.height == 720) {
				// Q5 Q10 keyboard
				minAlpha = 0.10f;
				alpha = minAlpha;
			}
		}
		Color tCol = this.guiTexture.color;
		tCol.a = alpha;
		this.guiTexture.color = tCol;
	}
	
	void Update () {
		if ((!press)&&(alpha > minAlpha)) {
			alpha = alpha - 0.01f;
			Color tCol = this.guiTexture.color;
			tCol.a = alpha;
			this.guiTexture.color = tCol;
		}
	}
}
