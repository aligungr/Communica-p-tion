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
        public Text id;
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
            string url = "http://localhost:64021/connectWithoutHololens";
            //  string a = null;
            byte[] array = null;
            HttpResponseHandler myHandler1 = (int statusCode, string responseText, byte[] responseData) =>
            {
                if (statusCode == 200)
                {
                    if (responseData != null)
                    {
                        userId = BitConverter.ToInt32(responseData, 0);
                        id.GetComponent<Text>().text = userId.ToString();
                        SceneManager.LoadScene("OptionsUI");
                    }
                }
               /* else
                {
                    Debug.Log(statusCode);
                    Debug.Log(responseText);
                }*/
            };
            HttpRequest.Send(this, "POST", url, null, array, myHandler1);
        }
        public void updateId(int newId)
        {
            if(newId != 0)
            {
                id.GetComponent<Text>().text = newId.ToString();
            }
            
        }
    }
}