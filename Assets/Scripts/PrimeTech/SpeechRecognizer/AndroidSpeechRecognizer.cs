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

        public static void Construct(RecognitionListenerProxy listenerProxy) {
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

                speechRecognizerInstance.Call("setRecognitionListener", listenerProxy);

                UnityLooper.Enqueue(() => {
                    Debug.Log("android speech recognizer constructed");
                });

                constructed = true;
            });
        }

        public static void StartListening() {
            var intent = createIntent();

            AndroidUtils.RunOnUiThread(() => {
                speechRecognizerInstance.Call("startListening", intent);
            });
        }

        private static AndroidJavaObject createIntent() {
            var intent = new AndroidJavaObject("android.content.Intent", "android.speech.action.RECOGNIZE_SPEECH");
            intent.Call<AndroidJavaObject>("putExtra", "calling_package", "com.primetech.communication");
            intent.Call<AndroidJavaObject>("putExtra", "android.speech.extra.PARTIAL_RESULTS", true);
            intent.Call<AndroidJavaObject>("putExtra", "android.speech.extra.LANGUAGE_MODEL", "free_form");
            intent.Call<AndroidJavaObject>("putExtra", "android.speech.extra.MAX_RESULTS", 10);
            return intent;
        }
    }
}
