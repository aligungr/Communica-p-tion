using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static HttpRequest;

namespace PrimeTech.Core
{
    public class OcrGaleryController : MonoBehaviour
    {
        int index = 0;
        public GameObject itemTemplate;
        public GameObject content;
        public GameObject ocrText;

        private WelcomeController welcome;
        public int userId;

        [SerializeField]
        private string mediaPath = "C://Users//User//Desktop//ocr-test2.json";
        [SerializeField]
        private List<OcrMedia> mediaList;

        private void loadOcrMedia()
        {
            string url = "http://37.148.210.36:8081/gallery?userId=1";
            byte[] array = null;
            string downloadData;
            HttpResponseHandler myHandler1 = (int statusCode, string responseText, byte[] responseData) =>
            {
                Debug.Log(statusCode);
                if (statusCode == 200)
                {
                    Debug.Log(responseText);
                    if (responseText != null)
                    {
                        downloadData = responseText;
                        //downloadData = downloadData.Substring(10, downloadData.Length - 12);
                        Debug.Log(downloadData);
                        mediaList = JsonConvert.DeserializeObject<List<OcrMedia>>(downloadData);
                        foreach (var item in mediaList)
                        {
                            addItem(item.name, item.picture, item.id);
                        }
                    }
                }
            };
            HttpRequest.Send(this, "GET", url, null, array, myHandler1);
            

            /*using (StreamReader r = new StreamReader(mediaPath))
            {
                string json = r.ReadToEnd();
                mediaList = JsonConvert.DeserializeObject<List<OcrMedia>>(json);
                foreach (var item in mediaList)
                {
                    Debug.Log(item.name);
                    addItem(item.name, item.picture, item.id);
                }
            }*/

        }
        void Start()
        {
            /*welcome = GameObject.FindObjectOfType<WelcomeController>();
            userId = int.Parse(welcome.id.text);
            Debug.Log(userId);*/
            loadOcrMedia();
        }

        public void addItem(string name, string image, string id)
        {
            if (mediaList[index].picture == "")
            {
                ocrText.GetComponent<Text>().text = mediaList[index].text;
            }
            else
            {
                ocrText.GetComponent<Text>().text = "";
            }
            var copy = Instantiate(itemTemplate);
            copy.transform.parent = content.transform;
            copy.transform.localPosition = Vector3.zero;

            copy.GetComponentInChildren<Text>().text = name;
            int copyOfIndex = index;

            byte[] imageBytes = Convert.FromBase64String(image);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            if (mediaList[index].picture != "")
            {
               copy.GetComponent<Image>().sprite = sprite;
            }
            //copy.GetComponent<Image>().sprite = sprite;


            copy.GetComponent<Button>().onClick.AddListener(() => {
                Debug.Log("Index number " + mediaList[copyOfIndex].name + copyOfIndex);
                Global.detailedItemId = int.Parse(mediaList[copyOfIndex].id);
                //open DetailsScene for copyOfIndex th object
            });
            index++;
        }
        public void clickedAddButton()
        {
            Debug.Log("Add object button clicked.");
            //SceneManager.LoadScene("AddOcrObjectScene");
        }
        public void clickedSearchButton()
        {
            Debug.Log("Search button clicked.");
            //SceneManager.LoadScene("SearchScene");
        }
        public void returnClicked()
        {
            string sceneName = Global.galleryReturnScene;
            SceneManager.LoadScene(sceneName);
        }

    }
}
