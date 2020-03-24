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

        /*private MainScreenUIController face;
        private WebCamTexture wb;

        ITextureSequence sequence;
        ITextureConverter converter;
        IDetectService detector;

        [SerializeField] GameObject FaceConsumer = null;
        IFaceListConsumer consumer = null;*/
        //FaceSceneBehavior fb;
        bool isFace;
        private void Awake()
        {
                this.text = GetComponent<Text>();
        }

        private void Start() {
            nativeLang = SettingsController.GetLanguage();
            foreignLang = SettingsController.GetForeignLanguage();
            subtitleTrigger = SettingsController.GetSubtitleTrigger();
            AndroidSpeechRecognizer.Construct(new ScreenRecognitionListenerProxy());
            AndroidSpeechRecognizer.StartListening();
            ///face = new MainScreenUIController();
            /*if ((int)subtitleTrigger == 2)
            {
                Debug.Log("geldi if e");
                Debug.Log("geldi 1");
                Debug.Log("geldi 2");
                isFace = GameObject.Find("World").GetComponent<FaceSceneBehavior>().faceDetected;
                if (isFace)
                {
                    Debug.Log("geldi 3 true face");
                    AndroidSpeechRecognizer.Construct(new ScreenRecognitionListenerProxy());
                    AndroidSpeechRecognizer.StartListening();
                }
                else
                {
                    Debug.Log("geldi 4 false face");
                }
            }
            else
            {
                AndroidSpeechRecognizer.Construct(new ScreenRecognitionListenerProxy());
                AndroidSpeechRecognizer.StartListening();
            }*/


            //isFace = fb.GetComponent<FaceSceneBehavior>().faceDetected;
            // Debug.Log("geldi face degeri: " + isFace);
            /* if (isFace)
             {
                 //Debug.Log("girdi face: " + isFace);
                 AndroidSpeechRecognizer.StartListening();
             }*/


        }

        public void SetTextOnScreen(string text) {
            subtitleTrigger = SettingsController.GetSubtitleTrigger();
            if ((int)subtitleTrigger == 2)
            {
                Debug.Log("geldi if awake");
                isFace = GameObject.Find("World").GetComponent<FaceSceneBehavior>().faceDetected;
                if (isFace)
                {
                    Debug.Log("geldi 3 true face awake ");
                    if (foreignLang.ToString() != nativeLang.ToString())
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
                    this.text.text = ":)";
                }

            }
            else 
            {
                if (foreignLang.ToString() != nativeLang.ToString())
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
}