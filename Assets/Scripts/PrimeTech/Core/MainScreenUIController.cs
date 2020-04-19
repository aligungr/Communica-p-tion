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
    public Button ManualSpeech;

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

    private void StartScreen()
    {
        display.texture = background;
    }

    // Start is called before the first frame update
    void Start()
    {
        if((int)SettingsController.GetSubtitleTrigger() != 1)
        {
            ManualSpeech.gameObject.SetActive(false);
        }
        StartScreen();
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
