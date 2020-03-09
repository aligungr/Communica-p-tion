using PrimeTech.Core;
using PrimeTech.Translator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScreenUIController : MonoBehaviour
{
    int currentCamIndex = 0;

    WebCamTexture tex;

    public RawImage display;
    public Text startStopText;
    public AspectRatioFitter fit;
    WebCamDevice device;

    public Text text;

    private void Awake()
    {
        this.text = GetComponent<Text>();
    }

    public void SettingsClicked()
    {
        SceneManager.LoadScene("OptionsUI");
    }

    public void StartStopCamClicked()
    {
        if(tex != null)
        {
            StopWebCam();
            startStopText.text = "Start Camera";
        }
        else
        {
            device = WebCamTexture.devices[currentCamIndex];
            tex = new WebCamTexture(device.name);
            display.texture = tex;
            
            float ratio = tex.width / tex.height;
            fit.aspectRatio = ratio;

            float antiRotate = -(270 - tex.videoRotationAngle) + 180;
 
            Quaternion quatRot = new Quaternion();
            quatRot.eulerAngles = new Vector3(0, 0, antiRotate);

            display.transform.rotation = quatRot;

            tex.Play();
            startStopText.text = "Stop Camera";
        }
        
    }

    private void StopWebCam()
    {
        display.texture = null;
        tex.Stop();
        tex = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartStopCamClicked();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
