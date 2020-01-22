using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Collections.Generic;

namespace PrimeTech.Core
{
    public class SettingsUIController : MonoBehaviour
    {
        public Dropdown mode;
        public Dropdown subtitleTrigger;
        public Dropdown translateLanguage;
        public Dropdown language;
        public Button apply;

        public void OnModeChange()
        {
            if (mode.value == 0)
                Debug.Log("mode value is Speech To Text");
            else if (mode.value == 1)
                Debug.Log("mode value is Text Detection");
            else
                Debug.Log("mode value is None");
        }

        public void OnSubtitleTriggerChange()
        {
            if (subtitleTrigger.value == 0)
                Debug.Log("subtitleTrigger value is Always On");
            else if(subtitleTrigger.value == 1)
                Debug.Log("subtitleTrigger value is If Face Detected");
            else
                Debug.Log("subtitleTrigger value is ManuallyTriggered");
        }

        public void OnTranslateLanguageChange()
        {
            if (translateLanguage.value == 0)
                Debug.Log("translateLanguage value is ON");
            else
                Debug.Log("translateLanguage value is OFF");
        }

        public void OnLanguagesChange()
        {
            if(language.value == 0)
                Debug.Log("languages value is Turkish");
            else
                Debug.Log("languages value is NOT Turkish");

        }

        public void OnClickApplyButton()
        {
            SaveSettings();
        }

        public void SaveSettings()
        {
            SettingsController.SetMode((Modes)mode.value);
            SettingsController.SetSubtitleTrigger((SubtitleTrigger)subtitleTrigger.value);
            SettingsController.SetTranslateLanguage((TranslateLanguage)translateLanguage.value);
            SettingsController.SetLanguage(Language.GetAllLanguages()[language.value]);
        }

        void Start()
        {
            mode.onValueChanged.AddListener(delegate { OnModeChange(); });
            subtitleTrigger.onValueChanged.AddListener(delegate { OnSubtitleTriggerChange(); });
            translateLanguage.onValueChanged.AddListener(delegate { OnTranslateLanguageChange(); });
            apply.onClick.AddListener(delegate { OnClickApplyButton(); });

            mode.value = (int)SettingsController.GetMode();
            subtitleTrigger.value = (int)SettingsController.GetSubtitleTrigger();
            translateLanguage.value = (int)SettingsController.GetTranslateLanguage();
            language.value = Language.GetAllLanguages().IndexOf(SettingsController.GetLanguage());
        }
    }

}
