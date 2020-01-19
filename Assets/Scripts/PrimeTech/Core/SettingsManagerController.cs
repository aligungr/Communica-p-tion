using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PrimeTech.Core
{
    public class SettingsManagerController : MonoBehaviour
    {
        public Dropdown mode;
        public Dropdown faceDetection;
        public Dropdown translateLanguage;
        public Button apply;
        // Start is called before the first frame update

        public void OnModeChange()
        {
            if (mode.options[mode.value].text.Equals("Speech To Text"))
                Debug.Log("mode value is Speech To Text");
            else if (mode.options[mode.value].text.Equals("Text Detection"))
                Debug.Log("mode value is Speech To Text Detection");
            else
                Debug.Log("mode value is None");
        }

        public void OnFaceDetectionChange()
        {
            if (faceDetection.options[faceDetection.value].text.Equals("OFF"))
                Debug.Log("faceDetection value is OFF");
            else
                Debug.Log("faceDetection value is ON");
        }

        public void OnTranslateLanguageChange()
        {
            if (translateLanguage.options[translateLanguage.value].text.Equals("OFF"))
                Debug.Log("translateLanguage value is OFF");
            else
                Debug.Log("translateLanguage value is ON");
        }

        public void OnClickApplyButton()
        {
            SaveSettings();
        }

        public void SaveSettings()
        {
            SettingsManager.SetMode(mode.value);
            SettingsManager.SetFaceDetection(faceDetection.value);
            SettingsManager.SetTranslateLanguage(translateLanguage.value);
        }

        void Start()
        {
            mode.onValueChanged.AddListener(delegate { OnModeChange(); });
            faceDetection.onValueChanged.AddListener(delegate { OnFaceDetectionChange(); });
            translateLanguage.onValueChanged.AddListener(delegate { OnTranslateLanguageChange(); });
            apply.onClick.AddListener(delegate { OnClickApplyButton(); });

            mode.value = SettingsManager.GetMode();
            faceDetection.value = SettingsManager.GetFaceDetection();
            translateLanguage.value = SettingsManager.GetTranslateLanguage();

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
