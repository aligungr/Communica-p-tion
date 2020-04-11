using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class CameraScript : MonoBehaviour
{
    static WebCamTexture backCam;
    // Start is called before the first frame update
    void Start()
    {
        if (backCam == null)
            backCam = new WebCamTexture(Screen.width, Screen.height);

        GetComponent<Renderer>().material.mainTexture = backCam;

        if (!backCam.isPlaying)
            backCam.Play();
    }

    public Texture2D TakeSnapshot()
    {

        Texture2D snap = new Texture2D(backCam.width, backCam.height);
        snap.SetPixels(backCam.GetPixels());
        snap.Apply();
        if (backCam.isPlaying)
        {
            backCam.Pause();
            //snap.ReadPixels(new Rect(0, 0, backCam.width, backCam.height), 0, 0);
            snap.Apply();
            //Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(snap, Application.productName + " Captures", "Capt_" + capture_count));


        }        
        return snap;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
