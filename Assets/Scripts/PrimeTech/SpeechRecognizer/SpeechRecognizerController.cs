using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PrimeTech.Translator;
using PrimeTech.Core;
using System;
using Alchera;
using System.Threading.Tasks;

namespace PrimeTech.SpeechRecognizer {

    public class SpeechRecognizerController : MonoBehaviour {

        private Text text;
        Translator.Translator translator;
        private Language nativeLang;
        private Language foreignLang;
        private SubtitleTrigger subtitleTrigger;
        private int translate;
        bool isFace;
        private void Awake()
        {
                this.text = GetComponent<Text>();
        }

        private void Start() {
            nativeLang = SettingsController.GetLanguage();
            foreignLang = SettingsController.GetForeignLanguage();
            translate = (int)SettingsController.GetTranslateLanguage();
            subtitleTrigger = SettingsController.GetSubtitleTrigger();
            if ((int)subtitleTrigger == 0) //Always on
            {
                AndroidSpeechRecognizer.Construct(new ScreenRecognitionListenerProxy());
                AndroidSpeechRecognizer.StartListening();
            }
            if ((int)subtitleTrigger == 2) //If face detected
            {
                AndroidSpeechRecognizer.Construct(new ScreenRecognitionListenerProxy());
                AndroidSpeechRecognizer.StartListening();
            }
        }
        private void Update() {
            subtitleTrigger = SettingsController.GetSubtitleTrigger();
            if ((int)subtitleTrigger == 2)
            {
                isFace = GameObject.Find("World").GetComponent<FaceSceneBehavior>().faceDetected; 
                AndroidSpeechRecognizer.StartListening();
            }
        }

        public void SetTextOnScreen(string text) {
            subtitleTrigger = SettingsController.GetSubtitleTrigger();
            if ((int)subtitleTrigger == 2)
            {
                if (isFace)
                {
                    if (foreignLang.ToString() != nativeLang.ToString() && translate == 0) //translate -> 0 = ON ,  translate -> 1 = OFF
                    {
                        translator = new Translator.Translator();

                        Action<Translator.Translator.Result> action = (Translator.Translator.Result result) =>
                        {
                            this.text.text = result.translatedText;
                        };
                        StartCoroutine(translator.translate(text, nativeLang, action));
                    }
                    else
                    {
                        this.text.text = text;
                    }
                }
                else
                {
                    this.text.text = "";
                }
            }
            else 
            {
                if (foreignLang.ToString() != nativeLang.ToString() && translate == 0) //translate -> 0 = ON ,  translate -> 1 = OFF
                {
                    translator = new Translator.Translator();

                    Action<Translator.Translator.Result> action = (Translator.Translator.Result result) =>
                    {
                        this.text.text = result.translatedText;
                        if ((int)SettingsController.GetSubtitleTrigger() == 0) //Always on
                            AndroidSpeechRecognizer.StartListening();
                    };
                    StartCoroutine(translator.translate(text, nativeLang, action));
                }
                else
                {
                    this.text.text = text;
                    if ((int)SettingsController.GetSubtitleTrigger() == 0) //Always on
                        AndroidSpeechRecognizer.StartListening();
                }
            }
        }
    }
}