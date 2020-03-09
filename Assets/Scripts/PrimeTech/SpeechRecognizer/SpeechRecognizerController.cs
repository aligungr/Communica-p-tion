using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PrimeTech.Translator;
using PrimeTech.Core;
using System;

namespace PrimeTech.SpeechRecognizer {

    public class SpeechRecognizerController : MonoBehaviour {

        private Text text;
        Translator.Translator translator;
        private Language nativeLang;
        private Language foreignLang;

        private void Awake() {
            this.text = GetComponent<Text>();
        }

        private void Start() {
            nativeLang = SettingsController.GetLanguage();
            foreignLang = SettingsController.GetForeignLanguage();
            //AndroidSpeechRecognizer.Construct(new DebugRecognitionListenerProxy());
            AndroidSpeechRecognizer.Construct(new ScreenRecognitionListenerProxy());

            AndroidSpeechRecognizer.StartListening();
        }

        public void SetTextOnScreen(string text) {
            if(foreignLang.ToString() != nativeLang.ToString())
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
    }
}