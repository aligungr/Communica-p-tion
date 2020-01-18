using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrimeTech.SpeechRecognizer {

    public class AndroidBundle {
        private readonly AndroidJavaObject obj;

        public AndroidBundle(AndroidJavaObject bundleObject) {
            this.obj = bundleObject;
        }

        public List<string> GetStringList(string key) {
            var result = new List<string>();

            var arrayList = obj.Call<AndroidJavaObject>("getStringArrayList", key);
            int size = arrayList.Call<int>("size");
            for (int i = 0; i < size; i++) {
                string str = arrayList.Call<string>("get", i);
                result.Add(str);
            }

            return result;
        }

        public float[] GetFloatArray(string key) {
            return obj.Call<float[]>("getFloatArray", key);
        }
    }
}

