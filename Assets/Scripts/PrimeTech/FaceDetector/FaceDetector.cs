using System.Collections;
using System.Collections.Generic;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using UnityEngine;

public class FaceDetector : MonoBehaviour
{ 
    // Start is called before the first frame update
    void Start()
    { 
        /*CascadeClassifier Classifier = new CascadeClassifier(Path.Combine(Application.dataPath, "Cascades//haarcascade_frontalface_default.xml"));
        System.Drawing.Bitmap img = new System.Drawing.Bitmap(Path.Combine(Application.dataPath, "ExampleImages//ex_5.jpg"));
        Image<Bgr, byte> picture = new Image<Bgr, byte>(img);
        System.Drawing.Rectangle[] rectangles = Classifier.DetectMultiScale(picture, 1.4, 0);
        foreach (System.Drawing.Rectangle rectangle in rectangles)
        {
            using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(img))
            {
                using (System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red, 1))
                {
                    gr.DrawRectangle(pen, rectangle);
                }
            }
            Debug.Log(string.Format("lu: {0}, {1} ru: {2}, {3} ld: {4}, {5} rd: {6}, {7}", rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Top, rectangle.Left, rectangle.Bottom, rectangle.Right, rectangle.Bottom));
        }
        img.Save("C:\\Users\\User\\Desktop\\ex_5_v2.jpg");*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
