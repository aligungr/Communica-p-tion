using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using static HttpRequest;
using UnityEngine.Networking;


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
        public GameObject disconnect;

        private PinScreenController pinScreen;
        private string pin;
        private Button disconnect_button;
        private int userId;



        public void AddOptionsToMode()
        {
            mode.AddOptions(new List<string>() {"Speech To Text","Text Detection"});
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
            translateLanguage.AddOptions(new List<string>() { "ON", "OFF"});
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
            else if(subtitleTrigger.value == 1)
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
        public void OnClickDisconnectButton()
        {
            string url = "http://localhost:64021/disconnectDevice" + userId;
            byte[] array = null;
            HttpResponseHandler myHandler1 = (int statusCode, string responseText, byte[] responseData) =>
            {
                if (statusCode == 200)
                {
                    if (responseData != null)
                    {
                        disconnect.SetActive(false);
                    }
                }
            };
            HttpRequest.Send(this, UnityWebRequest.kHttpVerbPOST, url, null, array, myHandler1);
        }
        public void SaveSettings()
        {
            SettingsController.SetMode((Modes)mode.value);
            SettingsController.SetLanguage(Language.GetAllLanguages()[language.value]);
            SettingsController.SetForeignLanguage(Language.GetAllLanguages()[foreignLanguage.value]);
            SettingsController.SetSubtitleTrigger((SubtitleTrigger)subtitleTrigger.value);
            SettingsController.SetTranslateLanguage((TranslateLanguage)translateLanguage.value);
            SceneManager.LoadScene("MainScreenUI");
            //Application.Quit();
        }
        public void SetDisconnectButton()
        {
            GameObject disconnectButton = (GameObject)Instantiate(disconnect);
            disconnect.SetActive(false);
            disconnect_button = GameObject.Find("Button_d").GetComponent<Button>();
            CheckIfConnected();
        }
        private void CheckIfConnected()
        {
            
            pinScreen = GameObject.FindObjectOfType<PinScreenController>();
            pin = pinScreen.pin;
            Debug.Log(pin);
            
            string url = "http://localhost:64021/checkPairing" + pin;
            byte[] array = null;
            HttpResponseHandler myHandler1 = (int statusCode, string responseText, byte[] responseData) =>
            {
                if (statusCode == 200)
                {
                    if (responseData != null)
                    {
                        userId = BitConverter.ToInt32(responseData, 0);
                        if (userId != 0)
                        {
                            disconnect.SetActive(true);
                        }
                    }     
                }
            };
            HttpRequest.Send(this, UnityWebRequest.kHttpVerbPOST, url, null, array, myHandler1);
        }
        void Start()
        {
            
            AddOptionsToMode();
            AddOptionsToLanguage();
            AddOptionsToForeignLanguage();
            AddOptionsToSubtitleTrigger();
            AddOptionsToTranslateLanguage();
            SetDisconnectButton();

            mode.onValueChanged.AddListener(delegate { OnModeChange(); });
            language.onValueChanged.AddListener(delegate { OnLanguagesChange(); });
            foreignLanguage.onValueChanged.AddListener(delegate { OnForeignLanguagesChange(); });
            subtitleTrigger.onValueChanged.AddListener(delegate { OnSubtitleTriggerChange(); });
            translateLanguage.onValueChanged.AddListener(delegate { OnTranslateLanguageChange(); });
            apply.onClick.AddListener(delegate { OnClickApplyButton(); });
            disconnect_button.onClick.AddListener(delegate { OnClickDisconnectButton(); });


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
