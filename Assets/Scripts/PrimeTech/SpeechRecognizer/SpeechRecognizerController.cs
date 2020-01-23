using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrimeTech.SpeechRecognizer {

    public class SpeechRecognizerController : MonoBehaviour {

        private Text text;

        private void Awake() {
            this.text = GetComponent<Text>();
        }

        private void Start() {
            //AndroidSpeechRecognizer.Construct(new DebugRecognitionListenerProxy());
            AndroidSpeechRecognizer.Construct(new ScreenRecognitionListenerProxy());

            AndroidSpeechRecognizer.StartListening();
        }

        public void SetTextOnScreen(string text) {
            this.text.text = text;
        }
    }
}