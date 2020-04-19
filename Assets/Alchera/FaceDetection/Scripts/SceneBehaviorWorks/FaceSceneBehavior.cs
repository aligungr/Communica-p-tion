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
                }
                translator.Dispose();
                Debug.Log("face state: " + faceDetected);
                //faceDetected = false;
            }
        }
        public void SettingsClicked()
        {
            SceneManager.LoadScene("OptionsUI");
        }
        public void goToGalery()
        { 
            SceneManager.LoadScene("GaleryScene");
        }

        public void goToOcrGalery()
        {
            SceneManager.LoadScene("OcrGaleryScene");
        }
        void OnDestroy()
        {
            sequence.Dispose();
            detector.Dispose();
        }

    }
}