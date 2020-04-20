using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using PrimeTech.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static HttpRequest;

public class NativeCam : MonoBehaviour
{
    //public Texture background;
    [SerializeField] private RawImage outputImage;
    string url;
    string json;
    bool mediaOrText;

    int artId;
    int userId; 
    int fileSize=0;
    bool mediaType=false;
    string data="";

    // Start is called before the first frame update

    void Start()
    {
        outputImage.enabled = false;
        userId=SettingsController.GetUserId();
        mediaOrText = Global.mediaOrText;
        artId= Global.detailedItemId;
        ResimCek(1024);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Eğer kamera meşgulse bir şey yapma
            if (NativeCamera.IsCameraBusy())
                return;

            if (Input.mousePosition.x < Screen.width / 2)
            {
                // Kamera ile resim çek
                // Eğer resmin genişliği veya yüksekliği 512 pikselden büyükse, resmi ufalt
                //ResimCek(512);
            }
            else
            {
                //ResimCek(512);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("OcrGaleryScene");
        }
    }

    private void ResimCek(int maksimumBuyukluk)
    {
        NativeCamera.Permission izin = NativeCamera.TakePicture((konum) =>
        {
            Debug.Log("Çekilen resmin konumu: " + konum);
            if (konum != null)
            {
                // Çekilen resmi bir Texture2D'ye çevir
                Texture2D texture = NativeCamera.LoadImageAtPath(konum, maksimumBuyukluk, false);
                if (texture == null)
                {
                    Debug.Log(konum + " konumundaki resimden bir texture oluşturulamadı.");
                    return;
                }
                outputImage.enabled = true;

                RectTransform rectTransform = outputImage.GetComponent<RectTransform>();
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                    rectTransform.rect.width * texture.height / texture.width);

                data = Convert.ToBase64String(texture.EncodeToJPG());
                fileSize = texture.EncodeToJPG().Length;
                outputImage.texture = texture;
                sendMedia();

                // 5 saniye sonra küp objesini yok et
                //Destroy(kup, 5f);

                // Küp objesi ile birlikte Texture2D objesini de yok et
                // Eğer prosedürel bir objeyi (Texture2D) işiniz bitince yok etmezseniz,
                // mevcut scene'i değiştirene kadar obje hafızada kalmaya devam eder
                //Destroy(texture, 5f);
            }
        }, maksimumBuyukluk);

        Debug.Log("İzin durumu: " + izin);
    }
    public void sendMedia()
    {
        if (!mediaOrText) //False ise Media kaydetsin
        {
            Debug.Log("Send Media");
            url = "http://37.148.210.36:8081/saveMediaMessage";
            //string json = "{\"ArtId\":\"" + artId + "\", \"UserId\":\"" + userId + "\",\"FileSize\":\"" + fileSize + "\",\"MediaType\":\"" + mediaType + "\",\"Data\":\"" + data + "\"}";
            string json = "{\n    \"ArtId\": " + artId + ",\n\t\"FileSize\": " + fileSize + ",\n    \"MediaType\": false,\n    \"UserId\": " + userId + ",\n    \"Data\": \"" +data+ "\"\n}";

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Content-Type", "application/json");
            HttpResponseHandler myHandler = (int statusCode, string responseText, byte[] responseData) =>
            {
                Debug.Log("status code");
                Debug.Log(responseText);
                Debug.Log(statusCode);
            };
            HttpRequest.Send(this, "POST", url, dict, json, myHandler);
            Debug.Log(json);
        }
        else // değilse text
        {
            url = "http://37.148.210.36:8081/saveTextMessage";
        
            JValue Data = new JValue(data);
         

            //string json = array.ToString();
            //string json = "{\n    \"ArtId\": 2,\n\t\"FileSize\": 12,\n    \"MediaType\": false,\n    \"UserId\": 3,\n    \"Data\": \"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\"\n}";
            string json = "{\n    \"ArtId\": "+artId+",\n\t\"FileSize\": "+fileSize+",\n    \"MediaType\": false,\n    \"UserId\": "+userId+",\n    \"Data\": \""+data+"\"\n}";

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Content-Type", "application/json");
            HttpResponseHandler myHandler = (int statusCode, string responseText, byte[] responseData) =>
            {
                Debug.Log("status code");
                Debug.Log(responseText);
                Debug.Log(statusCode);
            };
            HttpRequest.Send(this, "POST", url, dict, json, myHandler);
            Debug.Log(json);
        }
        
    }

    public void backToGallery()
    {
        Debug.Log("Butona Tıklandı");
        SceneManager.LoadScene("OcrGaleryScene");
    }
    public void YeniResimCek()
    {
        ResimCek(1024);

    }




}
