using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextTypewriter : MonoBehaviour
{

    TextMeshProUGUI textbox;

    int visibleCharacters = 0;
    [SerializeField] int charactersPerFrame = 2;
    [SerializeField] string tempText = "I'm going to use my divine knowledge to get away with this textbox, and idk what I mean by that";

    private void Awake()
    {
        textbox = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine("Typewrite", tempText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Typewrite(string text="No Text Here!")
    {
        visibleCharacters = 0;
        textbox.maxVisibleCharacters = 0;
        textbox.text = text;

        for (int i=0; i<=text.Length; i++)
        {
            textbox.maxVisibleCharacters = visibleCharacters;
            visibleCharacters+=charactersPerFrame;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
