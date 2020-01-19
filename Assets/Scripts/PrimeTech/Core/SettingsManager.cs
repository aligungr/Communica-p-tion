using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PrimeTech.Core
{
    public class SettingsManager : MonoBehaviour
    {
        public static void SetMode(int mode)
        {
            PlayerPrefs.SetInt("mode", mode);
        }

        public static int GetMode()
        {
            return PlayerPrefs.GetInt("mode");
        }

        public static void SetFaceDetection(int faceDetection)
        {
            PlayerPrefs.SetInt("faceDetection", faceDetection);
        }

        public static int GetFaceDetection()
        {
            return PlayerPrefs.GetInt("faceDetection");
        }

        public static void SetTranslateLanguage(int translateLanguage)
        {
            PlayerPrefs.SetInt("translateLanguage", translateLanguage);
        }

        public static int GetTranslateLanguage()
        {
            return PlayerPrefs.GetInt("translateLanguage");
        }

    }

}
