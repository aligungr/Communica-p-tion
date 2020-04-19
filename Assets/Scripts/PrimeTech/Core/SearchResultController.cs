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
    public class SearchResultController : MonoBehaviour
    {
        int index = 0;
        public GameObject itemTemplate;
        public GameObject content;

        public GameObject searchWord;

        private WelcomeController welcome;
        public int userId;

        [SerializeField]
        private string mediaPath = "C://Users//User//Desktop//ocr-test3.json";
        [SerializeField]
        private List<OcrMedia> mediaList;

        private void loadMedia()
        {
            /*
             * Search result data is obtained from the relevant service.
             */

            using (StreamReader r = new StreamReader(mediaPath))
            {
                string json = r.ReadToEnd();
                mediaList = JsonConvert.DeserializeObject<List<OcrMedia>>(json);
                foreach (var item in mediaList)
                {
                    Debug.Log(item.name);
                    addItem(item.name, item.picture, item.id);
                }
            }

        }
        void Start()
        {
            searchWord.GetComponent<Text>().text = Global.searchWord;   
            loadMedia();
        }

        public void addItem(string name, string image, string id)
        {
            var copy = Instantiate(itemTemplate);
            copy.transform.parent = content.transform;
            copy.transform.localPosition = Vector3.zero;

            copy.GetComponentInChildren<Text>().text = name;
            int copyOfIndex = index;

            byte[] imageBytes = Convert.FromBase64String(image);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

           // copy.GetComponent<Image>().sprite = sprite;

            copy.GetComponent<Button>().onClick.AddListener(() => {
                Debug.Log("Index number " + mediaList[copyOfIndex].name + copyOfIndex);
                Global.detailedItemId = int.Parse(mediaList[copyOfIndex].id);
                Global.detailsOrAdd = false;
                SceneManager.LoadScene("DetailsScene");
            });
            index++;
        }
    }
}