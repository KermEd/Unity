using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityGB;

public class DefaultEmulatorManager : MonoBehaviour
{
	public string Filename;
	public Renderer ScreenRenderer;
	
	// Add custom events

	public EmulatorBase Emulator
	{
		get;
		private set;
	}
	
	private Dictionary<KeyCode, EmulatorBase.Button> _keyMapping;
	
	void onLoadRom(string file) {
		Debug.Log ("Loading Rom " + file);
		StartCoroutine(LoadRom(file));
	}
	void Awake() {
		ENTERPressScript.Instance.onLoadRom += onLoadRom;
	}
	// Use this for initialization
	void Start()
	{

		// Init Keyboard mapping
		_keyMapping = new Dictionary<KeyCode, EmulatorBase.Button>();
		_keyMapping.Add(KeyCode.W, EmulatorBase.Button.Up);
		_keyMapping.Add(KeyCode.S, EmulatorBase.Button.Down);
		_keyMapping.Add(KeyCode.A, EmulatorBase.Button.Left);
		_keyMapping.Add(KeyCode.D, EmulatorBase.Button.Right);
		_keyMapping.Add(KeyCode.P, EmulatorBase.Button.A);
		_keyMapping.Add(KeyCode.O, EmulatorBase.Button.B);
		_keyMapping.Add(KeyCode.Return, EmulatorBase.Button.Start);
		_keyMapping.Add(KeyCode.Space, EmulatorBase.Button.Select);

		// Load emulator
		IVideoOutput drawable = new DefaultVideoOutput();
		IAudioOutput audio = GetComponent<DefaultAudioOutput>();
		ISaveMemory saveMemory = new DefaultSaveMemory();
		Emulator = new Emulator(drawable, audio, saveMemory);
		ScreenRenderer.material.mainTexture = ((DefaultVideoOutput) Emulator.Video).Texture;

		gameObject.audio.enabled = false;
		StartCoroutine(LoadRom(Filename));

		// Disable some features to save on battery
		Input.gyro.enabled = false;
		Input.compass.enabled = false;
	}

	void Update()
	{
		// Input
		foreach (KeyValuePair<KeyCode, EmulatorBase.Button> entry in _keyMapping)
		{
			if (this.transform.parent.transform.position.x == 0) {
				if (Input.GetKeyDown(entry.Key))
					Emulator.SetInput(entry.Value, true);
				else if (Input.GetKeyUp(entry.Key))
					Emulator.SetInput(entry.Value, false);
			}
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			byte[] screenshot = ((DefaultVideoOutput) Emulator.Video).Texture.EncodeToPNG();
			File.WriteAllBytes(LoaderScript.getPath() + "screenshot" + DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Day.ToString() + DateTime.Today.Hour.ToString() + DateTime.Today.Minute.ToString() + DateTime.Today.Second.ToString() + ".png", screenshot);
			Debug.Log("Screenshot saved. " + LoaderScript.getPath());
		}
	}



	private IEnumerator LoadRom(string filename)
	{
		LoaderScript.findRoms ();
		string path;
		path = "file://" + LoaderScript.getPath() + filename; 

		Debug.Log("Loading rom: " + path);
		WWW www = new WWW(path);
		yield return www;

		if (www.error == null)
		{
			Emulator.LoadRom(www.bytes);
			StartCoroutine(Run());
		} else 
			Debug.Log("Error during loading the rom.\n" + www.error);
	}

	private IEnumerator Run()
	{
		gameObject.audio.enabled = true;
		while (true)
		{			
			// Run
			Emulator.RunNextStep();

			yield return null;
		}
	}
}
