using Newtonsoft.Json;
using PrimeTech.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DetailsController : MonoBehaviour
{
    private GameObject[] ImgList;
    public GameObject ImgOne;
    public GameObject ImgTwo;
    public GameObject ImgThree;
    public GameObject ImgFour;
    public GameObject ImgFive;
    public GameObject ImgSix;

    [SerializeField]
    private string mediaPath = "C://Users//catal//Desktop//imges//test2.json";
    [SerializeField]
    private List<Media> mediaList;
    private void loadMedia()
    {
        /*string url = "http://localhost:64021/mediaItems/"+userId;
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
                        addItem(item.name, item.tumbnail, item.id, item.type);
                    }
                }
            }
        };
        HttpRequest.Send(this, "GET", url, null, array, myHandler1);*/

        using (StreamReader r = new StreamReader(mediaPath))
        {
            string json = r.ReadToEnd();
            mediaList = JsonConvert.DeserializeObject<List<Media>>(json);
            int index = 0;
            foreach (var media in mediaList)
            {
                addItems(media, ImgList[index] );
                index++;
            }
            
        }

    }

    private void addItems(Media media, GameObject ImgX)
    {
        byte[] imageBytes = Convert.FromBase64String(media.tumbnail);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imageBytes);
        Sprite spriteImg = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

        ImgX.GetComponent<Image>().sprite = spriteImg;
    }

    void Start()
    {

        fillImgList();
        /*welcome = GameObject.FindObjectOfType<WelcomeController>();
        userId = int.Parse(welcome.id.text);
        Debug.Log(userId);*/
        loadMedia();
    }

    void fillImgList()
    {
        ImgList = new GameObject[6];

        ImgOne = GameObject.Find("ImgOne");
        ImgTwo = GameObject.Find("ImgTwo");
        ImgThree = GameObject.Find("ImgThree");
        ImgFour = GameObject.Find("ImgFour");
        ImgFive = GameObject.Find("ImgFive");
        ImgSix = GameObject.Find("ImgSix");

        ImgList[0] = ImgOne;
        ImgList[1] = ImgTwo;
        ImgList[2] = ImgThree;
        ImgList[3] = ImgFour;
        ImgList[4] = ImgFive;
        ImgList[5] = ImgSix;     
    }

    public void addItem(string name, string image, string id, string type)
    {
        /*AndroidJavaClass jc = new AndroidJavaClass("android.os.Environment");
        string path = jc.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", jc.GetStatic<string>("DIRECTORY_DCIM")).Call<string>("getAbsolutePath");
        path = Path.Combine(path, "CommunicaptionMedias");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string path2 = path + name;
        if (type == "image")
        {
            path2 = path2 + ".jpg";
        }
        else if (type == "video")
        {
            path2 = path2 + ".mp4";
        }
        if (!File.Exists(path2))
        {
            downloaded.GetComponent<Image>().enabled = true;
        }
        else
        {
            downloaded.GetComponent<Image>().enabled = false;
        }*/

        //var copy = Instantiate(itemTemplate);
        //copy.transform.parent = content.transform;
        //copy.transform.localPosition = Vector3.zero;

        //copy.GetComponentInChildren<Text>().text = name;
        //int copyOfIndex = index;

        //byte[] imageBytes = Convert.FromBase64String(image);
        //Texture2D tex = new Texture2D(2, 2);
        //tex.LoadImage(imageBytes);
        //Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

        //copy.GetComponent<Image>().sprite = sprite;

        //copy.GetComponent<Button>().onClick.AddListener(() => {
        //    Debug.Log("Index number " + mediaList[copyOfIndex].name + copyOfIndex);
        //    downloadMedia(copyOfIndex);
        //});
        //index++;
    }
}
