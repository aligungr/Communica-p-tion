using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class CameraScript : MonoBehaviour
{
    static WebCamTexture backCam;
    int capture_count=0;
    // Start is called before the first frame update
    void Start()
    {
        if (backCam == null)
            backCam = new WebCamTexture();

        GetComponent<Renderer>().material.mainTexture = backCam;

        if (!backCam.isPlaying)
            backCam.Play();
    }

    //public void OnGUI()
    //{
      //  if (GUI.Button(new Rect(70, 70, 50, 30), "Click"))
        //    TakeSnapshot();
    //}

    //private string _SavePath = "/Users/apple/Desktop/Communica-p-tion/Assets/";
    //private int _CaptureCounter = 0;
    public Texture2D TakeSnapshot()
    {
        Texture2D snap = new Texture2D(backCam.width, backCam.height);
        snap.SetPixels(backCam.GetPixels());
        snap.Apply();
        if (backCam.isPlaying)
            backCam.Pause();
        File.WriteAllBytes("/Users/apple/Desktop/Communica-p-tion/" + capture_count.ToString()+ ".png", snap.EncodeToPNG());
        ++capture_count;
        return snap;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
