using PrimeTech;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace yandexapi
{ 
    //TODO- Google API'si ile karşılaştırılacak
    class YandexTranslator
    {
        public string Translate(string text, string translateTo)
        {
            LanguagesEnum enumObject = new LanguagesEnum();
            string lang = enumObject.GetType().GetField(translateTo).GetValue(enumObject);

            //Yandex automatically detects languages from given text.
            YandexTranslator translateObject = new YandexTranslator();
            string result = translateObject.TranslateText(text, translateTo);

            return result;
        }
        static void Main(string[] args)
        {
            //Program translateObject = new Program();
            //string result = translateObject.TranslateText("Tomaten.", "tr");
        }
        public string TranslateText(string text, string lang)
        {
            using (var wb = new WebClient())
            {
                var reqData = new NameValueCollection();
                reqData["text"] = text; // text to translate
                reqData["lang"] = lang; // target language
                //reqData["key"] = "trnsl.1.1.20200108T125628Z.7b77bb98c7754634.9e5d6a18d335a2610e6371bcb7e3fd4bd466a5cc";
                reqData["key"] = "";

                try
                {
                    var response = wb.UploadValues("https://translate.yandex.net/api/v1.5/tr.json/translate", "POST", reqData);
                    string responseInString = Encoding.UTF8.GetString(response);

                    var rootObject = JsonConvert.DeserializeObject<Translation>(responseInString);
                    Console.WriteLine($"Original text: {reqData["text"]}\n" +
                        $"Translated text: {rootObject.text[0]}\n" +
                        $"Lang: {rootObject.lang}");

                    Console.ReadLine();
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

    public class Translation
    {
        public int code { get; set; }
        public string lang { get; set; }
        public List<string> text { get; set; }
    }
}