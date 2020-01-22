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
            return (Modes) PlayerPrefs.GetInt("mode",(int)Modes.None);
        }

        public static void SetSubtitleTrigger(SubtitleTrigger subtitleTrigger)
        {
            PlayerPrefs.SetInt("subtitleTrigger", (int)subtitleTrigger);
        }

        public static SubtitleTrigger GetSubtitleTrigger()
        {
            return (SubtitleTrigger)PlayerPrefs.GetInt("subtitleTrigger", (int)SubtitleTrigger.AlwaysOn);
        }

        public static void SetTranslateLanguage(TranslateLanguage translateLanguage)
        {
            PlayerPrefs.SetInt("translateLanguage", (int)translateLanguage);
        }

        public static TranslateLanguage GetTranslateLanguage()
        {
            return (TranslateLanguage)PlayerPrefs.GetInt("translateLanguage",(int)TranslateLanguage.OFF);
        }

        public static void SetLanguage(Language language)
        {
            PlayerPrefs.SetInt("language", Language.GetAllLanguages().IndexOf(language));
        }

        public static Language GetLanguage()
        {
            return  Language.GetAllLanguages()[PlayerPrefs.GetInt("language", Language.GetAllLanguages().IndexOf(Language.Turkish))];
        }
    }

}
