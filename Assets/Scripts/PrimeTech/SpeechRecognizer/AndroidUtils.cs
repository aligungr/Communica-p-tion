using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrimeTech.SpeechRecognizer {

    public static class AndroidUtils {

        public static AndroidJavaObject GetActivityContext() {
            var jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            return jo;
        }
    }
}
