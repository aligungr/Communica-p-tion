using PrimeTech.Core;
using PrimeTech.Translator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Alchera;
using System.Threading.Tasks;

public class MainScreenUIController : MonoBehaviour
{
    int currentCamIndex = 0;

    public WebCamTexture tex;

    public RawImage display;
    public Text startStopText;
    public AspectRatioFitter fit;
    WebCamDevice device;

    public Text text;


    private SubtitleTrigger subtitleTrigger;
    ITextureSequence sequence;
    ITextureConverter converter;
    IDetectService detector;
    public bool faceDetected;

    private void Awake()
    {
        this.text = GetComponent<Text>();
    }

    public void SettingsClicked()
    {
        SceneManager.LoadScene("OptionsUI");
    }

    public  void StartStopCamClicked()
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

           /* subtitleTrigger = SettingsController.GetSubtitleTrigger();
            if ((int)subtitleTrigger == 2)
            {
                sequence = GetComponent<ITextureSequence>();
                converter = GetComponent<ITextureConverter>();
                detector = GetComponent<IDetectService>();
                Debug.Log("mainin icine geldi1");
                IEnumerable<FaceData> faces = null;
                var texture = tex;
                var image = await converter.Convert(texture);
                Debug.Log("mainin icine geldi2");
                var translator = await detector.Detect(ref image);
                Debug.Log("mainin icine geldi3");
                faces = translator.Fetch<FaceData>(faces);
                Debug.Log("mainin icine geldi4");
                if (faces != null)
                {
                    faceDetected = true;
                    translator.Dispose();
                }
                else
                {
                    faceDetected = false;
                    translator.Dispose();
                }
            }*/
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
