using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using static HttpRequest;
using System.Dynamic;

namespace PrimeTech.Core
{
    public class SettingsUIController : MonoBehaviour
    {
        public Dropdown mode;
        public Dropdown language;
        public Dropdown foreignLanguage;
        public Dropdown subtitleTrigger;
        public Dropdown translateLanguage;
        public Button apply;

        public void AddOptionsToMode()
        {
            mode.AddOptions(new List<string>() { "Speech To Text", "Text Detection" });
        }

        public void AddOptionsToLanguage()
        {
            language.AddOptions(typeof(Language).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => x.Name).ToList());
        }

        public void AddOptionsToForeignLanguage()
        {
            foreignLanguage.AddOptions(typeof(Language).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => x.Name).ToList());
        }

        public void AddOptionsToSubtitleTrigger()
        {
            subtitleTrigger.AddOptions(new List<string>() { "Always On", "Manually Triggered", "If Face Detected" });
        }

        public void AddOptionsToSubtitleTriggerWhenTextDetection()
        {
            subtitleTrigger.AddOptions(new List<string>() { "Always On", "Manually Triggered" });
        }

        public void AddOptionsToTranslateLanguage()
        {
            translateLanguage.AddOptions(new List<string>() { "ON", "OFF" });
        }

        public void OnModeChange()
        {
            if (mode.value == 1)
            {
                subtitleTrigger.interactable = true;
                subtitleTrigger.ClearOptions();
                AddOptionsToSubtitleTriggerWhenTextDetection();
                translateLanguage.interactable = false;
                language.interactable = false;
                foreignLanguage.interactable = false;
            }
            else
            {
                subtitleTrigger.interactable = true;
                subtitleTrigger.ClearOptions();
                AddOptionsToSubtitleTrigger();
                translateLanguage.interactable = true;
                language.interactable = true;
                foreignLanguage.interactable = true;
            }
        }

        public void OnLanguagesChange()
        {
            if (language.value == 0)
                Debug.Log("languages value is Turkish");
            else
                Debug.Log("languages value is NOT Turkish");

        }

        public void OnForeignLanguagesChange()
        {
            if (foreignLanguage.value == 0)
                Debug.Log("languages value is Turkish");
            else
                Debug.Log("languages value is NOT Turkish");

        }

        public void OnSubtitleTriggerChange()
        {
            if (subtitleTrigger.value == 0)
                Debug.Log("subtitleTrigger value is Always On");
            else if (subtitleTrigger.value == 1)
                Debug.Log("subtitleTrigger value is ManuallyTriggered");
            else
                Debug.Log("subtitleTrigger value is If Face Detected");
        }

        public void OnTranslateLanguageChange()
        {
            if (translateLanguage.value == 0)
                Debug.Log("translateLanguage value is ON");
            else
                Debug.Log("translateLanguage value is OFF");
        }

        public void OnClickApplyButton()
        {
            SaveSettings();
        }

        public void SaveSettings()
        {
            SettingsController.SetMode((Modes)mode.value);
            SettingsController.SetLanguage(Language.GetAllLanguages()[language.value]);
            SettingsController.SetForeignLanguage(Language.GetAllLanguages()[foreignLanguage.value]);
            SettingsController.SetSubtitleTrigger((SubtitleTrigger)subtitleTrigger.value);
            SettingsController.SetTranslateLanguage((TranslateLanguage)translateLanguage.value);

            string m = SettingsController.GetMode().ToString();
            string lang = SettingsController.GetLanguage().ToString();
            string foreign = SettingsController.GetForeignLanguage().ToString();
            string subtitle = SettingsController.GetSubtitleTrigger().ToString();
            string trans = SettingsController.GetTranslateLanguage().ToString();
             
            string url = "http://37.148.210.36:8081/saveSettingsMessage";
            string json = "{\"Mode\":\""+m+"\",\"NativeLanguageCode\":\"" + lang + "\",\"ForeignLanguageCode\":\"" + foreign + "\",\"SubtitleTrigger\":\"" + subtitle + "\",\"TranslateLanguage\":\"" + trans + "\"}";
            
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Content-Type", "application/json");

            HttpResponseHandler myHandler1 = (int statusCode, string responseText, byte[] responseData) =>
            {
                Debug.Log(responseText);
                Debug.Log(statusCode);
            };
            HttpRequest.Send(this, "POST", url, dict, json, myHandler1); 
            Debug.Log(json);

            if (subtitleTrigger.value == 2)
            {
                Global.galleryReturnScene = "Face2DFacemark";
                SceneManager.LoadScene("Face2DFacemark");
            }
            else
            {
                Global.galleryReturnScene = "MainScreenUI";
                SceneManager.LoadScene("MainScreenUI");
            }
            //SceneManager.LoadScene("MainScreenUI");
            //Application.Quit();
        }

        void Start()
        {
            AddOptionsToMode();
            AddOptionsToLanguage();
            AddOptionsToForeignLanguage();
            AddOptionsToSubtitleTrigger();
            AddOptionsToTranslateLanguage();

            mode.onValueChanged.AddListener(delegate { OnModeChange(); });
            language.onValueChanged.AddListener(delegate { OnLanguagesChange(); });
            foreignLanguage.onValueChanged.AddListener(delegate { OnForeignLanguagesChange(); });
            subtitleTrigger.onValueChanged.AddListener(delegate { OnSubtitleTriggerChange(); });
            translateLanguage.onValueChanged.AddListener(delegate { OnTranslateLanguageChange(); });
            apply.onClick.AddListener(delegate { OnClickApplyButton(); });

            mode.value = (int)SettingsController.GetMode();
            language.value = Language.GetAllLanguages().IndexOf(SettingsController.GetLanguage());
            foreignLanguage.value = Language.GetAllLanguages().IndexOf(SettingsController.GetForeignLanguage());
            subtitleTrigger.value = (int)SettingsController.GetSubtitleTrigger();
            translateLanguage.value = (int)SettingsController.GetTranslateLanguage();
        }

        void Update()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Application.Quit();
                }
            }
        }
    }

}