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
    public class Items
    {
        public List<Media> items;
    }
    public class AddObject : MonoBehaviour
    {
        int index = 0;
        public GameObject itemTemplate;
        public GameObject content;
        public GameObject downloaded;

        private WelcomeController welcome;
        public int userId;

        [SerializeField]
        private string mediaPath = "C://Users//User//Desktop//test2.json";
        [SerializeField]
        private List<Media> mediaList;

        private void loadMedia()
        {
             string url = "http://37.148.210.36:8081/mediaItems?userId=" + userId;
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
                         downloadData = downloadData.Substring(10, downloadData.Length-12);
                         Debug.Log(downloadData);
                         mediaList = JsonConvert.DeserializeObject<List<Media>>(downloadData);
                         foreach (var item in mediaList)
                         {
                              addItem(item.fileName, item.thumbnail, item.mediaId);
                         }
                     }   
                 } 
             };
             HttpRequest.Send(this, "GET", url, null, array, myHandler1);

        }
        void Start()
        {
            userId = SettingsController.GetUserId();
            loadMedia();
        } 

        public void addItem(string name, string image, string id)
        {
            AndroidJavaClass jc = new AndroidJavaClass("android.os.Environment");
            string path = jc.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", jc.GetStatic<string>("DIRECTORY_DCIM")).Call<string>("getAbsolutePath");
            path = Path.Combine(path, "CommunicaptionMedias");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string path2 = path + name + ".jpg";
             if (!File.Exists(path2))
             {
                 downloaded.GetComponent<Image>().enabled = true;
             }
             else
             {
                 downloaded.GetComponent<Image>().enabled = false;
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

            copy.GetComponent<Image>().sprite = sprite;

            copy.GetComponent<Button>().onClick.AddListener(() => { 
                Debug.Log("Index number " + mediaList[copyOfIndex].fileName + copyOfIndex);
                downloadMedia(copyOfIndex);
            });
            index++;
        }
        private void downloadMedia(int i)
        {
            string url = "http://37.148.210.36:8081/media?userId=" + userId+ "&mediaId=" + mediaList[i].mediaId;
            byte[] array = null;
            HttpResponseHandler myHandler1 = (int statusCode, string responseText, byte[] responseData) =>
            {
                if (statusCode == 200)
                {
                    if (responseData != null)
                    {
                        byte[] itemBGBytes = responseData;
                        AndroidJavaClass jc = new AndroidJavaClass("android.os.Environment");
                        string path = jc.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", jc.GetStatic<string>("DIRECTORY_DCIM")).Call<string>("getAbsolutePath");
                        path = Path.Combine(path, "CommunicaptionMedias");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string path2 = path + mediaList[i].fileName + ".jpg";
                        if (!File.Exists(path2))
                        {
                            File.WriteAllBytes(path2, itemBGBytes);
                            showAndroidToastMessage("Media saved successfully.");
                        }
                    }
                }
            };
            HttpRequest.Send(this, "GET", url, null, array, myHandler1);
        }
        private void showAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }

        public void returnClicked()
        {
            string sceneName = Global.galleryReturnScene;
            SceneManager.LoadScene(sceneName);
        }
    }
}