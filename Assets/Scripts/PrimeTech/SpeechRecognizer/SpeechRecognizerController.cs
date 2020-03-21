using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PrimeTech.Translator;
using PrimeTech.Core;
using System;
using System.Threading;

namespace PrimeTech.SpeechRecognizer {

    public class SpeechRecognizerController : MonoBehaviour {

        private Text text;
        Translator.Translator translator;
        private Language nativeLang;
        private Language foreignLang;
        private int translate;

        private void Awake() {
            this.text = GetComponent<Text>();
        }

        private void Start() {
            nativeLang = SettingsController.GetLanguage();
            foreignLang = SettingsController.GetForeignLanguage();
            translate = (int)SettingsController.GetTranslateLanguage();
            //AndroidSpeechRecognizer.Construct(new DebugRecognitionListenerProxy());
            AndroidSpeechRecognizer.Construct(new ScreenRecognitionListenerProxy());

            AndroidSpeechRecognizer.StartListening();
        }

        public void SetTextOnScreen(string text) {
            if(foreignLang.ToString() != nativeLang.ToString() && translate == 0) //translate -> 0 = ON ,  translate -> 1 = OFF
            {
                translator = new Translator.Translator();

                Action<Translator.Translator.Result> action = (Translator.Translator.Result result) =>
                {
                    this.text.text = result.translatedText;
                    Thread.Sleep(1000);
                    AndroidSpeechRecognizer.StartListening();
                };
                StartCoroutine(translator.translate(text, nativeLang, action));
            }
            else
            {
                this.text.text = text;
            }
        }
    }
}