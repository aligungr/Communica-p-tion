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
            string url = "http://37.148.210.36:8081/gallery?userId="+ userId;
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
                            addItem(item.title, item.picture, item.artId);
                        }
                    }
                }
            };
            HttpRequest.Send(this, "GET", url, null, array, myHandler1);

        }
        void Start()
        {
            userId = SettingsController.GetUserId();
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
            var copy = Instantiate(itemTemplate, content.transform);
            

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
                Debug.Log("Index number " + mediaList[copyOfIndex].title + copyOfIndex);
                Global.detailedItemId = int.Parse(mediaList[copyOfIndex].artId);
                Global.detailsOrAdd = false;
                SceneManager.LoadScene("DetailsScene");
            });
            index++;
        }
        public void clickedAddButton()
        {
            Global.detailsOrAdd = true;
            SceneManager.LoadScene("DetailsScene");
        }
        public void clickedSearchButton()
        {
            SceneManager.LoadScene("SearchScreen");
        }
        public void returnClicked()
        {
            string sceneName = Global.galleryReturnScene;
            SceneManager.LoadScene(sceneName);
        }

    }
}
