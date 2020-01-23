using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrimeTech.SpeechRecognizer {

    public class ScreenRecognitionListenerProxy : BaseRecognitionListenerProxy {

        public override void onBeginningOfSpeech() {
            Debug.Log("RecognitionListenerProxy|onBeginningOfSpeech");
        }

        public override void onEndOfSpeech() {
            Debug.Log("RecognitionListenerProxy|onEndOfSpeech");
        }

        public override void onError(int error) {
            Debug.Log("RecognitionListenerProxy|onError|" + error);
        }

        public override void onReadyForSpeech(AndroidJavaObject @params) {
            Debug.Log("RecognitionListenerProxy|onReadyForSpeech");
        }

        public override void onPartialResults(AndroidJavaObject partialResults) {
            var bundle = new AndroidBundle(partialResults);
            var recognized = bundle.GetStringList(SpeechRecognizerConstants.RESULTS_RECOGNITION);
            var confidence = bundle.GetFloatArray(SpeechRecognizerConstants.CONFIDENCE_SCORES);

            ShowOnScreen(recognized[0]);
        }

        public override void onResults(AndroidJavaObject results) {
            var bundle = new AndroidBundle(results);
            var recognized = bundle.GetStringList(SpeechRecognizerConstants.RESULTS_RECOGNITION);
            var confidence = bundle.GetFloatArray(SpeechRecognizerConstants.CONFIDENCE_SCORES);

            ShowOnScreen(recognized[0]);
        }

        private static void ShowOnScreen(string recognized) {
            GameObject.FindObjectOfType<SpeechRecognizerController>().SetTextOnScreen(recognized);
        }
    }
}
