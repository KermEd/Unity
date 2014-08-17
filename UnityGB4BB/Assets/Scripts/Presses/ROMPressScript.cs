using UnityEngine;
using System.Collections;

// ROM button for toggle
public class ROMPressScript : MonoBehaviour {

	private GUILayer guiHit;

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
					Debug.Log ("Go to roms");

					Vector3 tVec = this.transform.parent.parent.parent.transform.position;
					tVec.x = -3;
					this.transform.parent.parent.parent.transform.position = tVec;
				}
			}
		}
	}
}
