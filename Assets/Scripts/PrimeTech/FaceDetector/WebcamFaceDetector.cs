using UnityEngine;
using System.Collections;
using Emgu.CV;
using System.Drawing;
using System.IO;
using Emgu.CV.Structure;
using System;
using System.Text;
using UnityEngine.UI;

public class WebcamFaceDetector : MonoBehaviour
{
    public string deviceName;
    WebCamTexture wct;
    private float time = 0.0f;
    public float interpolationPeriod = 0.1f;

    // Use this for initialization
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        deviceName = devices[0].name;
        wct = new WebCamTexture(deviceName, 400, 300, 12);
        GetComponent<Renderer>().material.mainTexture = wct;
        wct.Play();
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= interpolationPeriod)
        {
            time = 0.0f;
            //faceDetection();
        }
    }


    /*void faceDetection()
    {
        Texture2D snap = new Texture2D(wct.width, wct.height);
        snap.SetPixels(wct.GetPixels());
        snap.Apply();
        MemoryStream ms = new MemoryStream(snap.EncodeToJPG());
        System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);

        CascadeClassifier Classifier = new CascadeClassifier(Path.Combine(Application.dataPath, "Cascades//haarcascade_frontalface_default.xml"));

        Bitmap img = new Bitmap(returnImage, returnImage.Size);

        Image<Bgr, byte> picture = new Image<Bgr, byte>(img);

        Rectangle[] rectangles = Classifier.DetectMultiScale(picture, 1.4, 0);
        foreach (Rectangle rectangle in rectangles)
        {
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(img))
            {
                using (Pen pen = new Pen(System.Drawing.Color.Red, 1))
                {
                    graphics.DrawRectangle(pen, rectangle);
                }
            }
        }
        MemoryStream stream = new MemoryStream();
        img.Save(stream, img.RawFormat);
        snap.LoadImage(stream.ToArray());
        GetComponent<Renderer>().material.mainTexture = snap;
    }*/

}