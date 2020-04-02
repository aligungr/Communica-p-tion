using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static HttpRequest;

namespace PrimeTech.Core
{
    public class AddObject : MonoBehaviour
    {
        int index = 0;
        public GameObject itemTemplate;
        public GameObject content;

        private WelcomeController welcome;
        public int userId;

        [SerializeField]
        private string mediaPath = "C://Users//User//Desktop//test2.json";
        [SerializeField]
        private List<Media> mediaList;

        private void loadMedia()
        {
            /*string url = "http://localhost:64021/mediaItems";
            byte[] array = null;
            string downloadData;
            HttpResponseHandler myHandler1 = (int statusCode, string responseText, byte[] responseData) =>
            {
                if (statusCode == 200)
                {
                    if (responseData != null)
                    {
                        downloadData = Encoding.UTF8.GetString(responseData, 0, responseData.Length);
                        mediaList = JsonConvert.DeserializeObject<List<Media>>(downloadData);
                        foreach (var item in mediaList)
                        {
                           // Debug.Log(item.name);
                            addItem(item.name, item.tumbnail, item.id);
                        }
                    }
                }
            };
            HttpRequest.Send(this, UnityWebRequest.kHttpVerbPOST, url, null, array, myHandler1);
           */

            using (StreamReader r = new StreamReader(mediaPath))
            {
                string json = r.ReadToEnd();
                mediaList = JsonConvert.DeserializeObject<List<Media>>(json);
                foreach (var item in mediaList)
                {
                    Debug.Log(item.name);
                    addItem(item.name, item.tumbnail, item.id);
                }
            }
        }
        void Start()
        {
            welcome = GameObject.FindObjectOfType<WelcomeController>();
            userId = int.Parse(welcome.id.text);
            Debug.Log(userId);
            loadMedia();

        }

        public void addItem(string name, string image, string id)
        {
            var copy = Instantiate(itemTemplate);
            copy.transform.parent = content.transform;
            copy.transform.localPosition = Vector3.zero;

            copy.GetComponentInChildren<Text>().text = " ";//name;
            int copyOfIndex = index;
            
            byte[] imageBytes = Convert.FromBase64String(image);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

            copy.GetComponent<Image>().sprite = sprite;

            copy.GetComponent<Button>().onClick.AddListener(() => { Debug.Log("Index number " + mediaList[copyOfIndex].name + copyOfIndex); });
            index++;
        }

    }

}