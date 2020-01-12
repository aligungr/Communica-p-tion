using PrimeTech;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace Translator
{ 
    //TODO - Compare to Google API
    class YandexTranslator
    {
        public string Translate(string text, string targetLanguage)
        {
            LanguagesEnum enumObject = new LanguagesEnum();
            string targetLang = enumObject.GetType().GetField(targetLanguage).GetValue(enumObject);

            //Yandex automatically detects language from given text.
            YandexTranslator translateObject = new YandexTranslator();  
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
                reqData["key"] = "";

                try
                {
                    var response = wb.UploadValues("https://translate.yandex.net/api/v1.5/tr.json/translate", "POST", reqData);
                    string responseInString = Encoding.UTF8.GetString(response);

                    var rootObject = JsonConvert.DeserializeObject<Translation>(responseInString);
                    /* Console.WriteLine($"Original text: {reqData["text"]}\n" +
                        $"Translated text: {rootObject.text[0]}\n" +
                        $"Lang: {rootObject.lang}");

                    Console.ReadLine(); */
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
    
    class Translation
    {
        public int code { get; set; }
        public string lang { get; set; }
        public List<string> text { get; set; }
    }
}