using PrimeTech.Core;
using PrimeTech.SpeechRecognizer;
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
    public Texture background;
    public Text startStopText;
    public AspectRatioFitter fit;
    public Button ManualSpeech;
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
        }
        
    }

    private void StopWebCam()
    {
        display.texture = background;
        tex.Stop();
        tex = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        if((int)SettingsController.GetSubtitleTrigger() != 1)
        {
            ManualSpeech.gameObject.SetActive(false);
        }
        StartStopCamClicked();
    }

    public void StartSpeechManually()
    {
        Debug.Log("Starting Speech Manually");
        AndroidSpeechRecognizer.Construct(new ScreenRecognitionListenerProxy());
        AndroidSpeechRecognizer.StartListening();
    }

    public void goToGalery()
    {
        SceneManager.LoadScene("GaleryScene");
    }

    public void goToOcrGalery()
    {
        SceneManager.LoadScene("OcrGaleryScene");
    }
    // Update is called once per frame
    void Update()
    {
    }
}
