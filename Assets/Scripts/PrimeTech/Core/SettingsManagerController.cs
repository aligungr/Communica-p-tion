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
            if (mode.value == 0)
                Debug.Log("mode value is NONE");
            else if (mode.value == 1)
                Debug.Log("mode value is TEXT DETECTION");
            else
                Debug.Log("mode value is SPEECH TO TEXT");
        }

        public void OnFaceDetectionChange()
        {
            if (faceDetection.value == 0)
                Debug.Log("faceDetection value is OFF");
            else if (faceDetection.value == 1)
                Debug.Log("faceDetection value is ON");
        }

        public void OnTranslateLanguageChange()
        {
            if (translateLanguage.value == 0)
                Debug.Log("translateLanguage value is OFF");
            else if (translateLanguage.value == 1)
                Debug.Log("translateLanguage value is ON");
        }

        public void OnClickApplyButton()
        {
            SaveSettings();
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetInt("mode", mode.value);
            PlayerPrefs.SetInt("faceDetection", faceDetection.value);
            PlayerPrefs.SetInt("translateLanguage", translateLanguage.value);
        }

        void Start()
        {
            mode.onValueChanged.AddListener(delegate { OnModeChange(); });
            faceDetection.onValueChanged.AddListener(delegate { OnFaceDetectionChange(); });
            translateLanguage.onValueChanged.AddListener(delegate { OnTranslateLanguageChange(); });
            apply.onClick.AddListener(delegate { OnClickApplyButton(); });

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
