using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TesseractDemoScript : MonoBehaviour
{
    public Texture2D imageToRecognize;
    [SerializeField] public Text displayText;
    [SerializeField] public RawImage outputImage;
    public WebCamera cam;
    private static readonly List<string> fileNames = new List<string> { "tessdata.tgz" };
    //[SerializeField] private Texture2D imageToRecognize;
    //[SerializeField] private Text displayText;
    //[SerializeField] private RawImage outputImage;


    private TesseractDriver _tesseractDriver;
    private string _text = "";
    private Texture2D _texture;


  

    private void Start()
    {
        //Texture2D texture = new Texture2D(imageToRecognize.width, imageToRecognize.height, TextureFormat.ARGB32, false);
        //texture.SetPixels32(imageToRecognize.GetPixels32());
        //texture.Apply();
        cam = new WebCamera();
        _tesseractDriver = new TesseractDriver();
        //Recognize(texture);
        //SetImageDisplay();

        //StartCamera(); semih

        //Texture2D texture = new Texture2D(imageToRecognize.width, imageToRecognize.height, TextureFormat.ARGB32, false);
        //texture.SetPixels32(imageToRecognize.GetPixels32());
        //texture.Apply();

        _tesseractDriver = new TesseractDriver();
        //Recognize(texture);
        //SetImageDisplay();
    }
    private void OnGUI()
    {


        //_tesseractDriver = new TesseractDriver();
        if (GUI.Button(new Rect(0, 180, 160, 30), "Click"))
        {
            imageToRecognize = cam.TakeSnapshot();
            Texture2D texture2 = new Texture2D(imageToRecognize.width, imageToRecognize.height, TextureFormat.ARGB32, false);
            texture2.SetPixels32(imageToRecognize.GetPixels32());
            texture2.Apply();
            if (imageToRecognize != null)
            {
                Recognize(texture2);
                //SetImageDisplay();

            }
        }
    }
    //private void StopWebCam()
    //{
    //    display.texture = null;
    //    tex.Stop();
    //    tex = null;

    //}

    private void Recognize(Texture2D outputTexture)
    {
        _texture = outputTexture;
        ClearTextDisplay();
        AddToTextDisplay(_tesseractDriver.CheckTessVersion());
        _tesseractDriver.Setup(OnSetupCompleteRecognize);
        //Debug.Log("Setup başarılı");
        //AddToTextDisplay(_tesseractDriver.Recognize(outputTexture));
        //AddToTextDisplay(_tesseractDriver.GetErrorMessage(), true);
    }
    private void OnSetupCompleteRecognize()
    {
        AddToTextDisplay(_tesseractDriver.Recognize(_texture));
        AddToTextDisplay(_tesseractDriver.GetErrorMessage(), true);
        SetImageDisplay();
     
        //AddToTextDisplay(_tesseractDriver.Recognize(outputTexture));
        //AddToTextDisplay(_tesseractDriver.GetErrorMessage(), true);
    }

    private void ClearTextDisplay()
    {
        _text = "";
    }

    private void AddToTextDisplay(string text, bool isError = false)
    {
        if (string.IsNullOrWhiteSpace(text)) return;

        _text += (string.IsNullOrWhiteSpace(displayText.text) ? "" : "\n") + text;

        if (isError)
            Debug.LogError(text);
        else
            Debug.Log(text);
    }

    private void LateUpdate()
    {
        if (displayText.text != null && _text != null)
            displayText.text = _text;
    }

    private void SetImageDisplay()
    {
        RectTransform rectTransform = outputImage.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            rectTransform.rect.width * _tesseractDriver.GetHighlightedTexture().height / _tesseractDriver.GetHighlightedTexture().width);
        outputImage.texture = _tesseractDriver.GetHighlightedTexture();
    }

}
