using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This acts as a central button press events, used to push presses to the emulator
public class pressButton : MonoBehaviour
{
	// Create singleton (single instance)
	private static pressButton instance;

	// Create our delegates
	public delegate void onClickEvent (string btn, bool isPressed); // Reference pointer to object
	public event onClickEvent onClick;

	public delegate void onClickGUIEvent (string btn, bool isPressed); // Reference pointer to object
	public event onClickEvent onClickGUI;
	
	// Dictionary for holding presses
	private Dictionary<string, bool> _buttons = new Dictionary<string, bool>();

	private GUILayer guiHit;

	// Setup singleton
	public static pressButton Instance {
		get {      
			if (instance == null)
				instance = GameObject.FindObjectOfType (typeof(pressButton)) as pressButton;     
			return instance;   
		}   
	}

	void Start ()
	{
		guiHit = Camera.main.GetComponent<GUILayer>();
		Debug.Log ("Building dictionary for presses.");
		_buttons.Add ("A", false);
		_buttons.Add ("B", false);
		_buttons.Add ("SR", false);
		_buttons.Add ("SE", false);
		_buttons.Add ("UP", false);
		_buttons.Add ("DN", false);
		_buttons.Add ("LT", false);
		_buttons.Add ("RT", false);
		_buttons.Add ("ROMPress", false);
		_buttons.Add ("GAMEPress", false);
	}

	void Update ()
	{
		// Create an empty dictionary clone to monitor for changes against primary
		Dictionary<string,bool> _buttonstmp = new Dictionary<string, bool>();

		_buttonstmp.Add ("A", false);
		_buttonstmp.Add ("B", false);
		_buttonstmp.Add ("SR", false);
		_buttonstmp.Add ("SE", false);
		_buttonstmp.Add ("UP", false);
		_buttonstmp.Add ("DN", false);
		_buttonstmp.Add ("LT", false);
		_buttonstmp.Add ("RT", false);
		_buttonstmp.Add ("ROMPress", false);
		_buttonstmp.Add ("GAMEPress", false);

		// Do multitouch
		if (Input.touchCount > 0) {
			// Loop through all touches
			foreach (Touch touch in Input.touches) {
				// Make sure its not a finger-up event
				if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) {
					// Check for touch
					GUIElement hitObj = guiHit.HitTest(touch.position);
					if (hitObj != null) {
						if (_buttonstmp.ContainsKey (hitObj.name)) {
							Debug.Log ("Press " + hitObj.name);
							_buttonstmp [hitObj.name] = true;
						}
					}
				} 
			}
		}
		// Now repeat for mouse devices
		if (Input.GetMouseButton(0)) {

			GUIElement hitObj = guiHit.HitTest(Input.mousePosition);
			if (hitObj != null) {
				if (_buttonstmp.ContainsKey (hitObj.name)) {
					Debug.Log ("Press " + hitObj.name);
					_buttonstmp [hitObj.name] = true;
				}
			}

		}

		// Now cleanup and compare lists
		foreach(KeyValuePair<string,bool> entry in _buttonstmp) {
			if (entry.Value != _buttons[entry.Key]) {
				Debug.Log("Event needs to be called for " + entry.Value);
				// mismatch, call event
				onClick (entry.Key,entry.Value);
				// update button
				onClickGUI (entry.Key,entry.Value);
				_buttons[entry.Key] = entry.Value;
			}
		}
	}
}