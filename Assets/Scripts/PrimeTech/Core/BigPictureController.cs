using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BigPictureController : MonoBehaviour
{
    public GameObject Img;

    // Start is called before the first frame update
    void Start()
    {
        byte[] imageBytes = Convert.FromBase64String(Global.bigResizedPicture);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imageBytes);
        Sprite spriteImg = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));

        Img.GetComponent<Image>().sprite = spriteImg;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("DetailsScene");
    }
}
