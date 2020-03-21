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

        public static void Construct(BaseRecognitionListenerProxy listenerProxy) {
#if !UNITY_EDITOR
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
#else
            Debug.Log("skipping initialization of Android Speech Recognizer on Editor");
#endif
        }

        public static void StartListening() {
#if !UNITY_EDITOR
            var intent = CreateIntent();

            AndroidUtils.RunOnUiThread(() => {
                speechRecognizerInstance.Call("startListening", intent);
            });
#else
            Debug.Log("start listening is ignored on Editor");
#endif
        }

        private static AndroidJavaObject CreateIntent() {
            Language foreignLanguage = SettingsController.GetForeignLanguage();
            var intent = new AndroidJavaObject("android.content.Intent", "android.speech.action.RECOGNIZE_SPEECH");
            intent.Call<AndroidJavaObject>("putExtra", "calling_package", "com.primetech.communication");
            intent.Call<AndroidJavaObject>("putExtra", "android.speech.extra.PARTIAL_RESULTS", false);
            intent.Call<AndroidJavaObject>("putExtra", "android.speech.extra.LANGUAGE_MODEL", "free_form");
            intent.Call<AndroidJavaObject>("putExtra", "android.speech.extra.SPEECH_INPUT_COMPLETE_SILENCE_LENGTH_MILLIS", 3000); //Wait 3 second before hearing.
            intent.Call<AndroidJavaObject>("putExtra", "android.speech.extra.SPEECH_INPUT_MINIMUM_LENGTH_MILLIS", 3000); //Wait 3 second before hearing.
            intent.Call<AndroidJavaObject>("putExtra", "android.speech.extra.SPEECH_INPUT_POSSIBLY_COMPLETE_SILENCE_LENGTH_MILLIS", 3000); //Wait 3 second before hearing.
            intent.Call<AndroidJavaObject>("putExtra", "android.speech.extra.MAX_RESULTS", 10);
            intent.Call<AndroidJavaObject>("putExtra", "android.speech.extra.ONLY_RETURN_LANGUAGE_PREFERENCE", true);
            intent.Call<AndroidJavaObject>("putExtra", "android.speech.extra.PREFER_OFFLINE", false);
            intent.Call<AndroidJavaObject>("putExtra", "android.speech.extra.LANGUAGE", foreignLanguage.ToString());

            
            return intent;
        }
    }
}
