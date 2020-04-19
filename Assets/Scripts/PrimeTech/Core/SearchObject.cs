using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SearchObject : MonoBehaviour
{
    public GameObject inputField;
    public Toggle spellCheck;

    public void Search()
    {
        string keyword = inputField.GetComponent<Text>().text;
        bool spellCheck = this.spellCheck.GetComponent<Toggle>().isOn;
        Global.searchWord = keyword;
        
        HttpRequest.HttpResponseHandler responseHandler = (int statusCode, string responseText, byte[] responseData) => {
            if (statusCode == 200)
            {
                Debug.Log(responseText);
                Global.searchResult = responseText;
                if (responseData != null)
                {
                    SceneManager.LoadScene("SearchResultScene");
                }
            }
        };

        dynamic flexible = new ExpandoObject();
        flexible.keyword = keyword;
        flexible.spellcheck = spellCheck;

        var dictionary = (IDictionary<string, object>)flexible;
        var serializedJson = JsonConvert.SerializeObject(dictionary);

        string url = "http://37.148.210.36:8081/search?json=" + serializedJson;

        byte[] array = null;
        HttpRequest.Send(this, "GET", url, null, array, responseHandler);
    }
}
