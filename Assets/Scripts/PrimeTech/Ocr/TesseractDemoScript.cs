using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TesseractDemoScript : MonoBehaviour {
	private Texture2D texture;
	public  Text text;
	public CameraScript cam = new CameraScript();
	public TesseractWrapper_And tesseract = new TesseractWrapper_And ();


	// Use this for initialization

	void Start () {
		// Files are not accessible in the .jar, so copy them to persistentDataPath
		CopyFile("tessdata/", "eng.cube.bigrams");
		CopyFile("tessdata/", "eng.cube.fold");
		CopyFile("tessdata/", "eng.cube.lm");
		CopyFile("tessdata/", "eng.cube.nn");
		CopyFile("tessdata/", "eng.cube.params");
		CopyFile("tessdata/", "eng.cube.size");
		CopyFile("tessdata/", "eng.cube.word-freq");
		CopyFile("tessdata/", "eng.tesseract_cube.nn");
		CopyFile("tessdata/", "eng.traineddata");
		CopyFile("tessdata/", "eng.user-patterns");
		CopyFile("tessdata/", "eng.user-words");
		CopyFile("tessdata/", "osd.traineddata");
		CopyFile("tessdata/", "pdf.ttf");
		CopyFile("tessdata/tessconfigs/", "debugConfigs.txt");
		CopyFile("tessdata/tessconfigs/", "recognitionConfigs.txt");
	
		//TesseractWrapper_And tesseract = new TesseractWrapper_And ();
		

		string datapath = System.IO.Path.Combine (Application.persistentDataPath, "tessdata");
		tesseract.Init ("eng", datapath);

		//string result = tesseract.RecognizeFromTexture (texture, false);

		//text.text = result ?? "Error: " + tesseract.errorMsg;
	}
    private void OnGUI()
    {
		if (GUI.Button(new Rect(180, 180, 90, 90), "Click"))
		{
			texture = cam.TakeSnapshot();
			string result = tesseract.RecognizeFromTexture(texture, false);
			if (result == null)
				text.text = "Text bulunamadı.";

			else
				text.text = result;
			Debug.Log(result);
		}   
	}
    void CopyFile(string folder, string file) {
		string fileUrl = System.IO.Path.Combine (Application.streamingAssetsPath, folder + file);
		string fileDirectory = System.IO.Path.Combine (Application.persistentDataPath, folder);
		string filePath = System.IO.Path.Combine (fileDirectory, file);
		Debug.Log("Copying: " + fileUrl);
		if(!Directory.Exists(fileDirectory)) {
			Directory.CreateDirectory(fileDirectory);
		}
		WWW www = new WWW(fileUrl);
		while(!www.isDone){
			Debug.Log("Reading");
		}
		File.WriteAllBytes(filePath, www.bytes);
		Debug.Log("file copy done ("+www.bytes.Length.ToString()+"): " + filePath);
		www.Dispose ();
		www = null;
	}
}
