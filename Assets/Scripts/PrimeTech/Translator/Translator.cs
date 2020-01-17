using LanguagesEnum;
using Newtonsoft.Json;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace Translator
{
    //TODO - Compare to Google API
    class Translator 
    {
        
        public string Translate(string text, string targetLanguage)
        {
            LanguagesEnumClass enumObject = new LanguagesEnumClass();
            string targetLang = enumObject.GetType().GetField(targetLanguage).GetValue(enumObject).ToString();

            //Yandex automatically detects language from given text.
            Translator translateObject = new Translator();
            string result = translateObject.TranslateText(text, targetLang);

            return result;
        }

        public string TranslateText(string text, string lang)
        {
            using (var wb = new WebClient())
            {
                var reqData = new NameValueCollection();
                reqData["text"] = text; // text to translate
                reqData["lang"] = lang; // target language
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
                    Console.WriteLine("ERROR!!! " + ex.Message);
                    throw;
                }

            }
        }
    }

    class GoogleTranslate
    {

    }

    [Serializable]
    class Translation
    {
        public int code { get; set; }
        public string lang { get; set; }
        public List<string> text { get; set; }
    }
}