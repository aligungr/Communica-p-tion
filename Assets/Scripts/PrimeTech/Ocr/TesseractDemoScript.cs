using System;
using UnityEngine;
using UnityEngine.UI;

public class TesseractDemoScript : MonoBehaviour
{
    [SerializeField] private Texture2D imageToRecognize;
    [SerializeField] private Text displayText;
    [SerializeField] private RawImage outputImage;
    private TesseractDriver _tesseractDriver;
    private string _text = "";


    int currentCamIndex = 0;

    WebCamTexture tex;

    public RawImage display;
    public AspectRatioFitter fit;
    WebCamDevice device;

    private Texture2D output;
    public Color[] data;

    public event Action OnSingleTap;
    public event Action OnDoubleTap;
    [Tooltip("Defines the maximum time between two taps to make it double tap")]
    [SerializeField] private float tapThreshold = 0.25f;
    private Action updateDelegate;
    private float tapTimer = 0.0f;
    private bool tap = false;
    private bool captureAlways;

    private void Awake()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        updateDelegate = UpdateEditor;
#elif UNITY_IOS || UNITY_ANDROID
        updateDelegate = UpdateMobile;
#endif
        OnDoubleTap = doubleClickCaptured;
    }



    private void OnDestroy()
    {
        OnSingleTap = null;
        OnDoubleTap = null;
    }

    private void Start()
    {
        StartCamera();

        Texture2D texture = new Texture2D(imageToRecognize.width, imageToRecognize.height, TextureFormat.ARGB32, false);
        texture.SetPixels32(imageToRecognize.GetPixels32());
        texture.Apply();

        _tesseractDriver = new TesseractDriver();
        Recognize(texture);
        SetImageDisplay();
    }


    private void doubleClickCaptured() //TODO: Will be odified according to options.
    {
        Debug.Log("Double Click Captured");
        if (captureAlways)
            captureAlways = false;
        else
            captureAlways = true;
    }

    private void StartCamera()
    {
        if (tex != null)
        {
            StopWebCam();
        }
        else
        {
            device = WebCamTexture.devices[currentCamIndex];
            tex = new WebCamTexture(device.name);
            display.texture = tex;


            float ratio = tex.width / tex.height;
            fit.aspectRatio = ratio;

            float antiRotate = -(270 - tex.videoRotationAngle) + 180;

            Quaternion quatRot = new Quaternion();
            quatRot.eulerAngles = new Vector3(0, 0, antiRotate);

            display.transform.rotation = quatRot;

            tex.Play();

            output = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
            //GetComponent<Renderer>().material.mainTexture = output;
            data = new Color[tex.width * tex.height];
            captureAlways = true;
        }
    }

    private void StopWebCam()
    {
        display.texture = null;
        tex.Stop();
        tex = null;
    }

    private void Recognize(Texture2D outputTexture)
    {
        ClearTextDisplay();
        AddToTextDisplay(_tesseractDriver.CheckTessVersion());
        _tesseractDriver.Setup();
        AddToTextDisplay(_tesseractDriver.Recognize(outputTexture));
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
        displayText.text = _text;
    }

    private void SetImageDisplay()
    {
        RectTransform rectTransform = outputImage.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            rectTransform.rect.width * _tesseractDriver.GetHighlightedTexture().height / _tesseractDriver.GetHighlightedTexture().width);
        outputImage.texture = _tesseractDriver.GetHighlightedTexture();
    }

    void Update()
    {
        if (updateDelegate != null) { updateDelegate(); }

        if (data != null && captureAlways) //disableCaptureOnUpdate if user wants double click capturing.
        {
            Color[] texData = tex.GetPixels();
            Texture2D takenPhoto = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
            takenPhoto.SetPixels(texData);
            takenPhoto.Apply();
            Recognize(takenPhoto);
            SetImageDisplay();
        }
    }

    public Texture2D GetOutput()
    {
        return output;
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    private void UpdateEditor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time < this.tapTimer + this.tapThreshold)
            {
                if (OnDoubleTap != null) { OnDoubleTap(); }
                this.tap = false;
                return;
            }
            this.tap = true;
            this.tapTimer = Time.time;
        }
        if (this.tap == true && Time.time > this.tapTimer + this.tapThreshold)
        {
            this.tap = false;
            if (OnSingleTap != null) { OnSingleTap(); }
        }
    }
#elif UNITY_IOS || UNITY_ANDROID
    private void UpdateMobile ()
    {
        for(int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if(Input.GetTouch(i).tapCount == 2)
                {
                    if(OnDoubleTap != null){ OnDoubleTap();}
                }
                if(Input.GetTouch(i).tapCount == 1)
                {
                    if(OnSingleTap != null) { OnSingleTap(); }
                }
            }
        }
    }
#endif

}