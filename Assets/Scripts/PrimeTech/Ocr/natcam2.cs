using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class natcam2 : MonoBehaviour
{
    [SerializeField] private RawImage outputImage;

    // Start is called before the first frame update
    void Start()
    {
        
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
                ResimCek(512);
            }
            else
            {
                // Kamera ile video kaydet
                VideoKaydet();
            }
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
                Texture2D texture = NativeCamera.LoadImageAtPath(konum, maksimumBuyukluk);
                if (texture == null)
                {
                    Debug.Log(konum + " konumundaki resimden bir texture oluşturulamadı.");
                    return;
                }

                // Texture'u geçici bir küp objesine ver
                //GameObject kup = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //kup.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 5f;
                //kup.transform.forward = -Camera.main.transform.forward;
                //kup.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                //Material material = kup.GetComponent<Renderer>().material;
                //if (!material.shader.isSupported) // eğer Standard shader desteklenmiyorsa Diffuse shader'ı kullan
                //    material.shader = Shader.Find("Legacy Shaders/Diffuse");

                RectTransform rectTransform = outputImage.GetComponent<RectTransform>();
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                    rectTransform.rect.width * texture.height / texture.width);
                outputImage.texture = texture;

                //material.mainTexture = texture;
                // 5 saniye sonra küp objesini yok et
                //Destroy(kup, 5f);

                // Küp objesi ile birlikte Texture2D objesini de yok et
                // Eğer prosedürel bir objeyi (Texture2D) işiniz bitince yok etmezseniz,
                // mevcut scene'i değiştirene kadar obje hafızada kalmaya devam eder
                Destroy(texture, 5f);
            }
        }, maksimumBuyukluk);

        Debug.Log("İzin durumu: " + izin);
    }

    private void VideoKaydet()
    {
        NativeCamera.Permission izin = NativeCamera.RecordVideo((konum) =>
        {
            Debug.Log("Kaydedilen videonun konumu: " + konum);
            if (konum != null)
            {
                // Videoyu oynat
                Handheld.PlayFullScreenMovie("file://" + konum);
            }
        });

        Debug.Log("İzin durumu: " + izin);
    }
}
