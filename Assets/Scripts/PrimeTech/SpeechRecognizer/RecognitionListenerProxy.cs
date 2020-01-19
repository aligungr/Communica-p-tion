using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrimeTech.SpeechRecognizer {

    public class RecognitionListenerProxy : AndroidJavaProxy {

        public RecognitionListenerProxy() : base("android.speech.RecognitionListener") {

        }

        /// <summary>
        /// The user has started to speak.
        /// </summary>
        public void onBeginningOfSpeech() {
            Debug.Log("RecognitionListenerProxy|onBeginningOfSpeech");
        }

        /// <summary>
        /// More sound has been received.
        /// </summary>
        /// <param name="buffer"></param>
        public void onBufferReceived(byte[] buffer) {
            
        }

        /// <summary>
        /// Called after the user stops speaking.
        /// </summary>
        public void onEndOfSpeech() {
            Debug.Log("RecognitionListenerProxy|onEndOfSpeech");
        }

        /// <summary>
        /// A network or recognition error occurred.
        /// </summary>
        /// <param name="error"></param>
        public void onError(int error) {
            Debug.Log("RecognitionListenerProxy|onError|" + error);
        }

        /// <summary>
        /// Reserved for adding future events.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="params"></param>
        public void onEvent(int eventType, AndroidJavaObject @params) {
        }

        /// <summary>
        /// Called when the endpointer is ready for the user to start speaking.
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public void onReadyForSpeech(AndroidJavaObject @params) {
            Debug.Log("RecognitionListenerProxy|onReadyForSpeech");
        }

        /// <summary>
        /// The sound level in the audio stream has changed.
        /// </summary>
        /// <param name="rmsdB"></param>
        public void onRmsChanged(float rmsdB) {

        }

        /// <summary>
        /// Called when partial recognition results are available.
        /// </summary>
        /// <param name="partialResults"></param>
        public void onPartialResults(AndroidJavaObject partialResults) {
            var bundle = new AndroidBundle(partialResults);
            var recognized = bundle.GetStringList(SpeechRecognizerConstants.RESULTS_RECOGNITION);
            var confidence = bundle.GetFloatArray(SpeechRecognizerConstants.CONFIDENCE_SCORES);

            Debug.Log("RecognitionListenerProxy|onPartialResults|" + recognized[0]);
        }

        /// <summary>
        /// Called when recognition results are ready.
        /// </summary>
        /// <param name="results"></param>
        public void onResults(AndroidJavaObject results) {
            var bundle = new AndroidBundle(results);
            var recognized = bundle.GetStringList(SpeechRecognizerConstants.RESULTS_RECOGNITION);
            var confidence = bundle.GetFloatArray(SpeechRecognizerConstants.CONFIDENCE_SCORES);

            Debug.Log("RecognitionListenerProxy|onResults|" + recognized[0]);
        }
    }
}
