using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeTech.Core;
using UnityEngine;

namespace PrimeTech.SpeechRecognizer {

    public static class AndroidSpeechRecognizer {
        private static AndroidJavaObject activityContext;
        private static AndroidJavaClass speechRecognizerClass;
        private static AndroidJavaObject speechRecognizerInstance;
        private static bool constructed;

        public static void Construct() {
            AndroidUtils.RunOnUiThread(() => {
                activityContext = AndroidUtils.GetActivityContext();
                speechRecognizerClass = new AndroidJavaClass("android.speech.SpeechRecognizer");

                if (!speechRecognizerClass.CallStatic<bool>("isRecognitionAvailable", activityContext)) {
                    UnityLooper.Enqueue(() => {
                        Debug.LogError("AndroidSpeechRecognizer: Recognition is not available");
                    });
                    return;
                }

                speechRecognizerInstance = speechRecognizerClass.CallStatic<AndroidJavaObject>("createSpeechRecognizer", activityContext);
                if (speechRecognizerInstance == null) {
                    UnityLooper.Enqueue(() => {
                        Debug.LogError("AndroidSpeechRecognizer: Recognition is not available");
                    });
                    return;
                }

                UnityLooper.Enqueue(() => {
                    Debug.Log("android speech recognizer constructed");
                });

                constructed = true;
            });
        }
    }
}
