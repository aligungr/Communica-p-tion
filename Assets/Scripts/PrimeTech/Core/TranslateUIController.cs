using PrimeTech.Translator;
using PrimeTech.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TranslateUIController : MonoBehaviour
{
    public Text textBox;

    private void Awake()
    {
        this.textBox = GetComponent<Text>();
    }

    public void ShowSubtitle()
    {
        textBox.text = "Translated Text Will be Here. Translated Text Will be Here. Translated Text Will be Here. Translated Text Will be Here.";
    }

    void Start()
    {
        ShowSubtitle();
    }

   
}
