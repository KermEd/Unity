using UnityEngine;
using System.Collections;
using System.IO;

public class LoaderScript : MonoBehaviour {

	static public string path = "";
	static public string[] roms;
	static public int romIndex;
	static public GUIText guiText;
	static public FileInfo[] fileInfo;

	// Prevent long strings and show only the end of the text string
	static public string mStr(string inString) {
		int maxLen = 28;
		if (inString.Length > maxLen) {
			inString = "..." + inString.Substring(inString.Length - maxLen);
		}
		return inString;
	}

	// Find all the roms in the folder and display it based on romIndex
	static public void findRoms() {

		updateText ("Loading...");

		if (romIndex == null)
			romIndex = 0;

		string extension = "*.gb";
		DirectoryInfo dirInfo = new DirectoryInfo(getPath());
		fileInfo = dirInfo.GetFiles(extension);

		string tString = "";

		int mIndex = romIndex + 10;
		tString = "<I>" + mStr("Path: " + getPath() + "\n\n") + "</I>";
		if (fileInfo.Length > mIndex) {
			for (int i = romIndex; i < mIndex; i++) {
				if (i == romIndex)
					tString = tString + "> ";
				else
					tString = tString + "  ";
				tString = tString + fileInfo[i].Name +"\n";
			}
			tString = tString + "... more";
		}
		else
		{
			for (int i = romIndex; i < fileInfo.Length; i++) {
				if (i == romIndex)
					tString = tString + "> ";
				tString = tString + fileInfo[i].Name +"\n";
			}
			tString = tString + "... end";
		}
		tString = tString + "\n\nQ Keys WSAD [Arrows], OP [Buttons]\nSpace & Enter, T Screenshot";

		updateText (tString);
	}

	// Update our text box
	static public void updateText(string text) {
		if (guiText == null)
			guiText = GameObject.Find("Text").guiText;
		guiText.text = text;	
	}

	// Get what path to use depending on platform
	static public string getPath() {
		if (path == "") {
			// Check platforms, change this to different platfoms as needed
			// I've set SAV games and ROMs to be in the same folder.
			switch (Application.platform) {
			case (RuntimePlatform.BlackBerryPlayer):
				path = "/accounts/1000/shared/misc/roms/gb/";
				break;
			default:
				path = System.Environment.CurrentDirectory + "\\Assets\\ROM\\";
				break;
			}
		}
		Debug.Log("ROM path set to: " + path);
		return path;
	}

	// Not used
	static public void createFolder(string path) {
		Debug.Log ("Creating path: " + path);
		File.Create (path);
	}
}
