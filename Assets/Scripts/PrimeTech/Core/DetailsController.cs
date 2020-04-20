using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrimeTech.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static HttpRequest;

public class DetailsController : MonoBehaviour
{
    private GameObject[] ImgList;
    public GameObject ImgOne;
    public GameObject ImgTwo;
    public GameObject ImgThree;
    public GameObject ImgFour;
    public GameObject ImgFive;
    public GameObject ImgSix;

    public GameObject OCRText;
    public GameObject WikiButton;
    public Text wikiText;

    public GameObject[] RecommendList;
    public GameObject RecommendationOne;
    public GameObject RecommendationTwo;
    public GameObject RecommendationThree;

    public GameObject ArtTitle;
    public GameObject ArtTitleLabel;
    public GameObject CreateArtButton;

    [SerializeField]
    private List<Media> mediaList;
    [SerializeField]
    private string ocrText;
    [SerializeField]
    private string wikipedia;
    [SerializeField]
    private List<Recommendation> recommendations;

    private void loadMedia()
    {
        string url = "http://37.148.210.36:8081/getDetails?artId=" + Global.detailedItemId.ToString();
        byte[] array = null;
        string dataString;
       
        HttpResponseHandler myHandler1 = (int statusCode, string responseText, byte[] responseData) =>
        {
            Debug.Log(statusCode);
            Debug.Log(responseText);
            Debug.Log(responseData);
            if (statusCode == 200)
            {
                if (responseData != null)
                {
                    dataString = Encoding.UTF8.GetString(responseData, 0, responseData.Length);
                    var detailsItem = JsonConvert.DeserializeObject<List<DetailsItem>>(responseText);

                    mediaList = detailsItem[0].items;
                    ocrText= detailsItem[0].text;
                    wikipedia= detailsItem[0].wikipedia;
                    recommendations = detailsItem[0].recommendations;

                    int index = 0;
                    foreach (var media in mediaList)
                    {
                        addItems(media, ImgList[index]);
                        index++;
                    }

                    for (int i = index; i < 6; i++)
                        ImgList[i].SetActive(false);

                    if (ocrText.Length == 0)
                        OCRText.SetActive(false);
                    else
                        OCRText.GetComponent<Text>().text = ocrText;

                    if (wikipedia.Length == 0)
                        WikiButton.SetActive(false);
                    else
                        wikiText.text = wikipedia;

                    if (recommendations.Count == 0)
                    {
                        RecommendationOne.SetActive(false);
                        RecommendationTwo.SetActive(false);
                        RecommendationThree.SetActive(false);
                    }
                    else
                    {
                        int indexRec = 0;
                        foreach (var recommendation in recommendations)
                        {
                            addRecommendation(recommendation, RecommendList[indexRec]);
                            indexRec++;
                        }

                        if(indexRec == 1)
                        {
                            RecommendationTwo.SetActive(false);
                            RecommendationThree.SetActive(false);
                        }
                        else if (indexRec == 2)
                            RecommendationThree.SetActive(false);

                    }
                        
                }
            }
        };
        HttpRequest.Send(this, "POST", url, null, array, myHandler1);
    }

    private void addItems(Media media, GameObject ImgX)
    {
        byte[] imageBytes = Convert.FromBase64String(media.thumbnail);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imageBytes);
        Sprite spriteImg = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));

        ImgX.GetComponent<Image>().sprite = spriteImg;
    }

    private void addRecommendation(Recommendation recommendation, GameObject ImgX)
    {
        byte[] imageBytes = Convert.FromBase64String(recommendation.picture);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imageBytes);
        Sprite spriteImg = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));

        ImgX.GetComponent<Image>().sprite = spriteImg;
    }

    void Start()
    {   
        fillImgList();
        OCRText = GameObject.Find("OCRText");
        WikiButton = GameObject.Find("WikiButton");
        RecommendationOne = GameObject.Find("RecommendationOne");
        RecommendationTwo = GameObject.Find("RecommendationTwo");
        RecommendationThree = GameObject.Find("RecommendationThree");
        ArtTitleLabel = GameObject.Find("ArtTitleLabel");
        CreateArtButton = GameObject.Find("CreateArtButton");
        if(Global.detailsOrAdd == false)
        {
            ArtTitle = GameObject.Find("ArtTitle");
            showDetailsScene();
            loadMedia();
        }
            
        else
            hideDetailsScene();
    }

    public void createNewArt()
    {
        string artTitle = ArtTitle.GetComponent<Text>().text;
        string userId = SettingsController.GetUserId().ToString();
        Debug.Log(artTitle);

        string url = "http://37.148.210.36:8081/createArt?userId=" + userId + "&artTitle=" + artTitle;
        byte[] array = null;

        HttpResponseHandler myHandler1 = (int statusCode, string responseText, byte[] responseData) =>
        {
            Debug.Log(statusCode);
            Debug.Log(responseText);
            if (statusCode == 200)
            {
                if (responseData != null)
                {
                    var artId = (int)JObject.Parse(responseText)["artId"];
                    Global.detailedItemId = artId;
                    Global.detailsOrAdd = false;
                    SceneManager.LoadScene("Ocr");
                }
            }
        };
        HttpRequest.Send(this, "POST", url, null, array, myHandler1);
    }

    void hideDetailsScene()
    {
        foreach (var ImgBtn in ImgList)
        {
            ImgBtn.SetActive(false);
        }

        OCRText.SetActive(false);
        WikiButton.SetActive(false);
        RecommendationOne.SetActive(false);
        RecommendationTwo.SetActive(false);
        RecommendationThree.SetActive(false);
        GameObject.Find("AddOcr").SetActive(false);
        GameObject.Find("AddPhoto").SetActive(false);
        GameObject.Find("Recommendations").SetActive(false);
        GameObject.Find("ExtraInfo").SetActive(false);

        ArtTitle.SetActive(true);
        ArtTitleLabel.SetActive(true);
        CreateArtButton.SetActive(true);
    }

    void showDetailsScene()
    {
        foreach (var ImgBtn in ImgList)
        {
            ImgBtn.SetActive(true);
        }

        OCRText.SetActive(true);
        WikiButton.SetActive(true);
        RecommendationOne.SetActive(true);
        RecommendationTwo.SetActive(true);
        RecommendationThree.SetActive(true);
        GameObject.Find("AddOcr").SetActive(true);
        GameObject.Find("AddPhoto").SetActive(true);
        GameObject.Find("Recommendations").SetActive(true);
        GameObject.Find("ExtraInfo").SetActive(true);

        ArtTitle.SetActive(false);
        ArtTitleLabel.SetActive(false);
        CreateArtButton.SetActive(false);
    }

    void fillImgList()
    {
        ImgList = new GameObject[6];
        RecommendList = new GameObject[3];

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

        RecommendationOne = GameObject.Find("RecommendationOne");
        RecommendationTwo = GameObject.Find("RecommendationTwo");
        RecommendationThree = GameObject.Find("RecommendationThree");

        RecommendList[0] = RecommendationOne;
        RecommendList[1] = RecommendationTwo;
        RecommendList[2] = RecommendationThree;
    }

    public void addPhoto()
    {
        Global.mediaOrText = false;
        SceneManager.LoadScene("Ocr");
    }

    public void addOcr()
    {
        Global.mediaOrText = true;
        SceneManager.LoadScene("Ocr");
    }

    public void openWikipedi()
    {
        Debug.Log(wikiText.text);
        Application.OpenURL(wikiText.text);
    }

    public void openRecommendationOne()
    {
        Debug.Log(recommendations[0].url);
        Application.OpenURL(recommendations[0].url);
    }

    public void openRecommendationTwo()
    {
        Debug.Log(recommendations[1].url);
        Application.OpenURL(recommendations[1].url);
    }

    public void openRecommendationThree()
    {
        Debug.Log(recommendations[2].url);
        Application.OpenURL(recommendations[2].url);
    }

    public void ImageOneClicked()
    {
        if(mediaList.Count != 0)
        {
            Global.bigResizedPicture = mediaList[0].thumbnail;
            SceneManager.LoadScene("BigPictureScene");
        }
       
    }
    public void ImageTwoClicked()
    {
        if (mediaList.Count != 0)
        {
            Global.bigResizedPicture = mediaList[1].thumbnail;
            SceneManager.LoadScene("BigPictureScene");
        }
    }
    public void ImageThreeClicked()
    {
        if (mediaList.Count != 0)
        {
            Global.bigResizedPicture = mediaList[2].thumbnail;
            SceneManager.LoadScene("BigPictureScene");
        }
    }
    public void ImageFourClicked()
    {
        if (mediaList.Count != 0)
        {
            Global.bigResizedPicture = mediaList[3].thumbnail;
            SceneManager.LoadScene("BigPictureScene");
        }
    }
    public void ImageFiveClicked()
    {
        if (mediaList.Count != 0)
        {
            Global.bigResizedPicture = mediaList[4].thumbnail;
            SceneManager.LoadScene("BigPictureScene");
        }
    }
    public void ImageSixClicked()
    {
        if (mediaList.Count != 0)
        {
            Global.bigResizedPicture = mediaList[5].thumbnail;
            SceneManager.LoadScene("BigPictureScene");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("OcrGaleryScene");
    }

}
