using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTech.SpeechRecognizer;
using UnityEngine;

namespace PrimeTech.Core {
    public class Startup : MonoBehaviour {
        public void Awake() {
            //Application.logMessageReceived += logDelegate;
            Application.logMessageReceivedThreaded += logDelegate;
        }

        private void logDelegate(string condition, string stackTrace, LogType type) {
            var console = FindObjectOfType<ConsoleController>();
            if (console != null) {
                console.AppendLog(condition, type);
            }
        }

        private void Start() {
            // Write your startup codes here

        }
    }
}

