using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRealTime : MonoBehaviour
{
    WebCamTexture webcam;
    public Texture2D output;
    public Color32[] data;
    IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            webcam = new WebCamTexture();
            webcam.Play();
            output = new Texture2D(webcam.width, webcam.height);
            GetComponent<Renderer>().material.mainTexture = output;
            data = new Color32[webcam.width * webcam.height];
        }
    }
    void Update()
    {
        if (data != null)
        {
            webcam.GetPixels32(data);
            // You can play around with data however you want here.
            // Color32 has member variables of a, r, g, and b. You can read and write them however you want.
            output.SetPixels32(data);
            output.Apply();
        }
    }

    public Texture2D GetOutput()
    {
        return output;   
    }
}
