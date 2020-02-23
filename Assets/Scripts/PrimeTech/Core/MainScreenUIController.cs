using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenUIController : MonoBehaviour
{
    int currentCamIndex = 0;

    WebCamTexture tex;

    public RawImage display;
    public Text startStopText;

    public void SwapCamClicked()
    {
        if (WebCamTexture.devices.Length > 0)
        {
            currentCamIndex += 1;
            currentCamIndex %= WebCamTexture.devices.Length;

            if(tex != null)
            {
                StopWebCam();
                StartStopCamClicked();
            }
        }
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
            WebCamDevice device = WebCamTexture.devices[currentCamIndex];
            tex = new WebCamTexture(device.name);
            display.texture = tex; 

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
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
