using UnityEngine;
using System.Collections;

public class ENTERPressScript : MonoBehaviour {

	// Create singleton (single instance)
	private static ENTERPressScript instance;
	
	// Create our delegates
	public delegate void onLoadRomEvent (string filename); // Reference pointer to object
	public event onLoadRomEvent onLoadRom;

	private GUILayer guiHit;
	private bool isKeyDown = false;
	private KeyCode keyCode = KeyCode.Return;
	
	public static ENTERPressScript Instance {
		get {      
			if (instance == null)
				instance = GameObject.FindObjectOfType (typeof(ENTERPressScript)) as ENTERPressScript;     
			return instance;   
		}   
	}

	// Use this for initialization
	void Start () {
		guiHit = Camera.main.GetComponent<GUILayer>();
	}

	public void loadRom() {
		Debug.Log ("Loading Rom:  " +  LoaderScript.fileInfo[LoaderScript.romIndex].Name);
		onLoadRom (LoaderScript.fileInfo[LoaderScript.romIndex].Name);
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
		if (LoaderScript.romIndex < LoaderScript.fileInfo.Length) {
			loadRom();	
			Vector3 tVec = this.transform.parent.parent.parent.transform.position;
			tVec.x = 0;
			this.transform.parent.parent.parent.transform.position = tVec;
		}
	}
}
