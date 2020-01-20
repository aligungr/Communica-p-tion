using UnityEngine;
using static PrimeTech.Core.SettingsManagerController;

namespace PrimeTech.Core
{
    public static class SettingsManager
    {
        public static void SetMode(Modes mode)
        {
            PlayerPrefs.SetInt("mode", (int)mode);
        }

        public static int GetMode()
        {
            return PlayerPrefs.GetInt("mode");
        }

        public static void SetFaceDetection(SubtitleTrigger faceDetection)
        {
            PlayerPrefs.SetInt("subtitleTrigger", (int)faceDetection);
        }

        public static int GetFaceDetection()
        {
            return PlayerPrefs.GetInt("subtitleTrigger");
        }

        public static void SetTranslateLanguage(TranslateLanguage translateLanguage)
        {
            PlayerPrefs.SetInt("translateLanguage", (int)translateLanguage);
        }

        public static int GetTranslateLanguage()
        {
            return PlayerPrefs.GetInt("translateLanguage");
        }

    }

}
