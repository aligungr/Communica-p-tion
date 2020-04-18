using UnityEngine;
using UnityEngine.UI;

public class WebCamera: MonoBehaviour
{
    int currentCamIndex = -1;
    static WebCamTexture tex;

    public RawImage display;

    public void SwapCamClicked()
    {
        Debug.Log("swithce tıklandı");
        if (WebCamTexture.devices.Length > 0)
        {
            currentCamIndex += 1;
            currentCamIndex %= WebCamTexture.devices.Length;
            Debug.Log("switche tıklandı");
        }
    }
    public void StartStopCamClicked()
    {
        if (tex != null)
        {
            display.texture = null;
            tex.Stop();
            tex = null;
        }
        else
        {
            WebCamDevice device = WebCamTexture.devices[currentCamIndex];
            tex = new WebCamTexture(device.name);
            display.texture = tex;
            tex.Play();
        }

    }
    public Texture2D TakeSnapshot()
    {

        Texture2D snap = new Texture2D(tex.width, tex.height);
        snap.SetPixels(tex.GetPixels());
        snap.Apply();
        if (tex.isPlaying)
        {
            tex.Pause();
            //snap.ReadPixels(new Rect(0, 0, backCam.width, backCam.height), 0, 0);
            snap.Apply();
            //Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(snap, Application.productName + " Captures", "Capt_" + capture_count));


        }
        return snap;

    }
    void Start()
    {
         SwapCamClicked();
         StartStopCamClicked();
    }

    // Update is called once per frame
    void Update()
    {

    }

}