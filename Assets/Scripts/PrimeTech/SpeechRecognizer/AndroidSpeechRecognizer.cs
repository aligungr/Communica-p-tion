using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrimeTech.SpeechRecognizer {

    public class AndroidSpeechRecognizer {
        private readonly AndroidJavaObject activityContext;
        private readonly AndroidJavaClass speechRecognizerClass;
        private readonly AndroidJavaObject speechRecognizerInstance;

        public AndroidSpeechRecognizer() {
            this.activityContext = AndroidUtils.GetActivityContext();
            this.speechRecognizerClass = new AndroidJavaClass("android.speech.SpeechRecognizer");

            if (!this.speechRecognizerClass.CallStatic<bool>("isRecognitionAvailable", activityContext)) {
                throw new Exception("AndroidSpeechRecognizer: Recognition is not available");
            }

            this.speechRecognizerInstance = this.speechRecognizerClass.CallStatic<AndroidJavaClass>("createSpeechRecognizer", activityContext);
            if (this.speechRecognizerInstance == null) {
                throw new Exception("AndroidSpeechRecognizer: Recognition is not available");
            }

            Debug.Log("android speech recognizer constructed");
        }
    }
}
