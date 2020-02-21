using Newtonsoft.Json;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using PrimeTech.Core;

namespace PrimeTech.Translator
{
    //TODO - Compare to Google API
    public class Translator : MonoBehaviour
    {

        public string Translate(string text, Language language)
        {
            //Yandex automatically detects language from given text.
            Translator translateObject = new Translator();
            string result = translateObject.TranslateText(text, language);

            return result;
        }

        public string TranslateText(string text, Language language)
        {
            using (var wb = new WebClient())
            {
                var reqData = new NameValueCollection();
                reqData["text"] = text; // text to translate
                reqData["lang"] = language.ToString(); // target language
                reqData["key"] = "API-Key Here";

                try
                {
                    var response = wb.UploadValues("https://translate.yandex.net/api/v1.5/tr.json/translate", "POST", reqData);
                    string responseInString = Encoding.UTF8.GetString(response);
                    var rootObject = JsonConvert.DeserializeObject<Translation>(responseInString);

                    return rootObject.text[0];
                }
                catch (Exception ex)
                {
                    Debug.LogError("ERROR!!! " + ex.Message);
                    throw;
                }

            }
        }
    }

    class GoogleTranslate
    {

    }

    class Translation
    {
        public int code { get; set; }
        public string lang { get; set; }
        public List<string> text { get; set; }
    }
}