﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrimeTech.SpeechRecognizer {

    public abstract class BaseRecognitionListenerProxy : AndroidJavaProxy {

        public BaseRecognitionListenerProxy() : base("android.speech.RecognitionListener") {

        }

        /// <summary>
        /// The user has started to speak.
        /// </summary>
        public virtual void onBeginningOfSpeech() { }

        /// <summary>
        /// More sound has been received.
        /// </summary>
        /// <param name="buffer"></param>
        public virtual void onBufferReceived(byte[] buffer) { }

        /// <summary>
        /// Called after the user stops speaking.
        /// </summary>
        public virtual void onEndOfSpeech() { }

        /// <summary>
        /// A network or recognition error occurred.
        /// </summary>
        /// <param name="error"></param>
        public virtual void onError(int error) { }

        /// <summary>
        /// Reserved for adding future events.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="params"></param>
        public virtual void onEvent(int eventType, AndroidJavaObject @params) { }

        /// <summary>
        /// Called when the endpointer is ready for the user to start speaking.
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public virtual void onReadyForSpeech(AndroidJavaObject @params) { }

        /// <summary>
        /// The sound level in the audio stream has changed.
        /// </summary>
        /// <param name="rmsdB"></param>
        public virtual void onRmsChanged(float rmsdB) { }

        /// <summary>
        /// Called when partial recognition results are available.
        /// </summary>
        /// <param name="partialResults"></param>
        public virtual void onPartialResults(AndroidJavaObject partialResults) { }

        /// <summary>
        /// Called when recognition results are ready.
        /// </summary>
        /// <param name="results"></param>
        public virtual void onResults(AndroidJavaObject results) { }
    }
}
