using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrimeTech.Core {

    public class ConsoleController : MonoBehaviour {
        private Text text;

        private void Awake() {
            text = GetComponent<Text>();
            text.text += "\n";
        }

        public void AppendLog(string message, LogType type) {
            string str = "<color=";
            switch (type) {
                case LogType.Exception:
                case LogType.Error:
                    str += "red";
                    break;
                case LogType.Warning:
                    str += "orange";
                    break;
                case LogType.Log:
                case LogType.Assert:
                default:
                    str += "white";
                    break;
            }

            str += ">";
            str += message;
            str += "</color>";

            text.text += str + "\n";
        }
    }
}
