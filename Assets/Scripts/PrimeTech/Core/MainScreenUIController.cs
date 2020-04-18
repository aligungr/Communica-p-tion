using PrimeTech.Core;
using PrimeTech.SpeechRecognizer;
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
    public Texture background;
    public Button ManualSpeech;

    public Text text;

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

    // Update is called once per frame
    void Update()
    {
    }
}
