using UnityEngine;
using System.Collections;

// User presses page down
public class PDNPressScript : MonoBehaviour {

	private GUILayer guiHit;
	private bool isKeyDown = false;
	private KeyCode keyCode = KeyCode.S;
	// Use this for initialization
	void Start () {
		guiHit = Camera.main.GetComponent<GUILayer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			GUIElement hitObj = guiHit.HitTest(Input.mousePosition);
			if (hitObj != null) {
				if (hitObj.name == this.name) {
					Debug.Log ("Press PDN");
					process();
				}
			}
		}
	
		if(Input.GetKeyDown(keyCode) && (this.transform.parent.parent.parent.transform.position.x == -3)) 
		{
			if(isKeyDown == false) 
			{
				isKeyDown = true;
				process();
			}
		}
		else
		{
			isKeyDown = false;
		}
	}
	void process(){
		if ((LoaderScript.romIndex < (LoaderScript.fileInfo.Length-1)) && (LoaderScript.fileInfo != null)) {
			LoaderScript.romIndex = LoaderScript.romIndex + 1;
			LoaderScript.findRoms();
		}
	}
}
