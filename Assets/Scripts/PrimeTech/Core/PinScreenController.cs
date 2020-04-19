using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static HttpRequest;

namespace PrimeTech.Core
{
    public class UserId
    {
        public string userId;
    }
    public class PinScreenController : MonoBehaviour
    {
        public GameObject pinArea;
        public Button buttonPin;
        public GameObject pinScreen;
        private WelcomeController welcome;
        private string pin;
        public int userId;

        // Start is called before the first frame update
        void Start()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            welcome = GameObject.FindObjectOfType<WelcomeController>();
            buttonPin.onClick.AddListener(validatePin);
        }

        private void validatePin()
        {
            pin = pinArea.GetComponent<InputField>().text;
            Debug.Log(pin);          
            string url = "http://37.148.210.36:8081/connectWithHololens?pin=" + pin;
            byte[] array = null;
            HttpResponseHandler myHandler1 = (int statusCode, string responseText, byte[] responseData) =>
             {
                 Debug.Log(statusCode);
                 if (statusCode == 200)
                 {
                     if (responseData != null)
                     {
                         var p = JsonConvert.DeserializeObject<UserId>(responseText);
                         userId = int.Parse(p.userId); 
                         SettingsController.SetUserId(userId);
                         Debug.Log(SettingsController.GetUserId());
                         SceneManager.LoadScene("OptionsUI");
                     } 
                     else
                     {
                         SceneManager.LoadScene("Welcome");
                     }
                 }
             };
            HttpRequest.Send(this, "POST", url, null, array, myHandler1);
        }
    }
}