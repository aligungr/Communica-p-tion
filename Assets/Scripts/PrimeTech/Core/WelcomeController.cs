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
    public class WelcomeController : MonoBehaviour
    {
        public Button buttonWithHololens;
        public Button buttonWithoutHololens;
        public GameObject welcomeScreen;
        //public Text id;
        public int userId;

        // Start is called before the first frame update
        void Start()
        {
            buttonWithHololens.onClick.AddListener(continueWithHololens);
            buttonWithoutHololens.onClick.AddListener(continueWithoutHololens);
        }
        private void continueWithHololens()
        {
            SceneManager.LoadScene("Pin");
        }
        private void continueWithoutHololens()
        {
            string url = "http://37.148.210.36:8081/connectWithoutHololens";
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