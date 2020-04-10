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
            string url = "http://localhost:64021/connectWithHololens" + pin;
            // string a = null;
            byte[] array = null;
            HttpResponseHandler myHandler1 = (int statusCode, string responseText, byte[] responseData) =>
             {
                 if (statusCode == 200)
                 {
                     if (responseData != null)
                     {
                         userId = BitConverter.ToInt32(responseData, 0);
                         welcome.updateId(userId);
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