using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private string mediaPath = "C://Users//User//Desktop//test-result.json";
        [SerializeField]
        private List<Result> mediaList;
 
        private void loadMedia()
        {
            string json = Global.searchResult;
            Debug.Log(json);
            //Debug.Log(json); 
            string downloadData = json.Substring(12, json.Length - 15);
            Debug.Log(downloadData); ;
            mediaList = JsonConvert.DeserializeObject<List<Result>>(downloadData); 
            foreach (var item in mediaList)
            {
                //Debug.Log(item.ArtId);
                addItem(item.Text, item.ArtId);
            }

        }
        void Start()
        {
            searchWord.GetComponent<Text>().text = Global.searchWord;   
            loadMedia();
        }

        public void addItem(string text, string ArtId)
        {
            var copy = Instantiate(itemTemplate, content.transform);

            copy.GetComponentInChildren<Text>().text = text;
            int copyOfIndex = index;

            copy.GetComponent<Button>().onClick.AddListener(() => {
                Debug.Log("Index number " + mediaList[copyOfIndex].ArtId + copyOfIndex);
                Global.detailedItemId = int.Parse(mediaList[copyOfIndex].ArtId);
                Global.detailsOrAdd = false;
                SceneManager.LoadScene("DetailsScene");
            });
            index++;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("SearchScene");
        }
    }
}