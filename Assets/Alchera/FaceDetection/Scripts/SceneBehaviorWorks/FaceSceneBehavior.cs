using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Alchera
{
    public class FaceSceneBehavior : MonoBehaviour
    {
        ITextureSequence sequence;
        ITextureConverter converter;
        IDetectService detector;

        [SerializeField] GameObject FaceConsumer = null;
        IFaceListConsumer consumer = null;

        public bool faceDetected = true;
        public Text text;

        private void Awake()
        {
            this.text = GetComponent<Text>();
        }
        public async void Start()
        {
            sequence = GetComponent<ITextureSequence>();
            converter = GetComponent<ITextureConverter>();
            detector = GetComponent<IDetectService>();
            consumer = FaceConsumer.GetComponent<IFaceListConsumer>();

            IEnumerable<FaceData> faces = null;

            foreach (Task<Texture> request in sequence.Repeat())
            {
                if (request == null)
                    continue;

                var texture = await request; 
                var image = await converter.Convert(texture);
                var translator = await detector.Detect(ref image);

                faces = translator.Fetch<FaceData>(faces);
                if (faces != null)
                {
                    consumer.Consume(ref image, faces);
                }
                else
                {
                    faceDetected = false;
                    Debug.Log("face falseeeeeeeee"); 
                }
                //cizmesin diye inactive ettim!!!
                /*if(faces != null)
                {
                    faceDetected = true;
                    Debug.Log("face trueee");
                    translator.Dispose();
                    //break;
                }
                else
                {
                    faceDetected = false;
                    Debug.Log("face falseee"); 
                    translator.Dispose();
                    //break;
                }*/
                
                Debug.Log("face durum: "+ faceDetected +" mus");
                faceDetected = false;
            }
        }
        public void SettingsClicked()
        {
            SceneManager.LoadScene("OptionsUI");
        }
        /*public async void detectFace()
        {
            sequence = GetComponent<ITextureSequence>();
            converter = GetComponent<ITextureConverter>();
            detector = GetComponent<IDetectService>();
            consumer = FaceConsumer.GetComponent<IFaceListConsumer>();

            IEnumerable<FaceData> faces = null;

            foreach (Task<Texture> request in sequence.Repeat())
            {
                if (request == null)
                    continue;

                var texture = await request;
                var image = await converter.Convert(texture);
                var translator = await detector.Detect(ref image);

                faces = translator.Fetch<FaceData>(faces);
                //if (faces != null) consumer.Consume(ref image, faces); cizmesin diye inactive ettim!!!
                if (faces != null)
                {
                    faceDetected = true;
                    translator.Dispose();
                    break;
                }
                else
                {
                    faceDetected = false;
                    translator.Dispose();
                    break;
                }
            }
        }*/ 

        void OnDestroy()
        {
            sequence.Dispose();
            detector.Dispose();
        }

    }
}

