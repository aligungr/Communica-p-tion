using UnityEngine;

namespace PrimeTech.Core
{
    public static class SettingsController
    {
        public static void SetMode(Modes mode)
        {
            PlayerPrefs.SetInt("mode", (int)mode);
        }

        public static Modes GetMode()
        {
            return (Modes) PlayerPrefs.GetInt("mode");
        }

        public static void SetSubtitleTrigger(SubtitleTrigger subtitleTrigger)
        {
            PlayerPrefs.SetInt("subtitleTrigger", (int)subtitleTrigger);
        }

        public static SubtitleTrigger GetSubtitleTrigger()
        {
            return (SubtitleTrigger)PlayerPrefs.GetInt("subtitleTrigger");
        }

        public static void SetTranslateLanguage(TranslateLanguage translateLanguage)
        {
            PlayerPrefs.SetInt("translateLanguage", (int)translateLanguage);
        }

        public static TranslateLanguage GetTranslateLanguage()
        {
            return (TranslateLanguage)PlayerPrefs.GetInt("translateLanguage");
        }

    }

}
