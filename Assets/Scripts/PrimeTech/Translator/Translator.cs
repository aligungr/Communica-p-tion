using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using PrimeTech.Core;
using System.Collections;
using System.Xml;

namespace PrimeTech.Translator
{
    public class Translator
    {

        public class Result
        {
            public const int NETWORK_ERROR = -1;
            public const int SUCCESS = 200;
            public const int INVALID_API_KEY = 401;
            public const int BLOCKED_API_KEY = 402;
            public const int DAILY_REQUESTS_EXCEEDED = 403;
            public const int DAILY_TEXT_EXCEEDED = 404;
            public const int TEXT_LENGTH_EXCEEDED = 413;
            public const int CANNOT_TRANSLATE = 422;
            public const int UNSUPPORTED_DIRECTION = 501;

            public readonly int status;
            public readonly Language language;
            public readonly string translatedText;

            public Result(int status, Language language, string translatedText)
            {
                this.status = status;
                this.language = language;
                this.translatedText = translatedText;
            }
        }


        public IEnumerator translate(string text, Language to, Action<Result> callback)
        {
            string encodedText = UnityWebRequest.EscapeURL(text);
            string apiKey = "trnsl.1.1.20200402T194016Z.bae2a9e6da51aa1e.311ec0e6e487ac3b01be327b4b136a8596c318ed";
            string requestUrl = string.Format("https://translate.yandex.net/api/v1.5/tr/translate?key={0}&text={1}&lang={2}", apiKey, encodedText, to.ToString());
            UnityWebRequest request = UnityWebRequest.Get(requestUrl);
            yield return request.SendWebRequest();
            Result result;
            if (request.isNetworkError)
            {
                result = new Result(Result.NETWORK_ERROR, to, null);
            }
            else
            {
                string responseText = request.downloadHandler.text;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseText);

                XmlNode translation = doc.SelectSingleNode("Translation");
                XmlNode error = doc.SelectSingleNode("Error");

                int status;
                string translatedText;

                if (translation != null)
                {
                    status = int.Parse(translation.Attributes["code"].Value);
                    translatedText = doc.SelectSingleNode("Translation/text").InnerText;
                }
                else
                {
                    status = int.Parse(error.Attributes["code"].Value);
                    translatedText = null;
                }
                result = new Result(status, to, translatedText);
            }
            callback(result);
        }

    }
}