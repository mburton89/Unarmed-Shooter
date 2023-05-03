using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class TextTypewriter : MonoBehaviour
{

    TextMeshProUGUI textbox;
    Image textbox_IMG;
    Image sprite_IMG;

    int visibleCharacters = 0;
    int currentDialogueNumber = 0;
    bool inDialogue = false;

    TextData thisDialogueList;
    public static TextTypewriter instance;

    public UnityEvent m_StartedDialogue;
    public UnityEvent m_EndedDialogue;

    [SerializeField] int charactersPerFrame = 2;


    private void Awake()
    {
        textbox = GetComponent<TextMeshProUGUI>();
        textbox_IMG = GetComponentInParent<Image>();
        sprite_IMG = GetComponentInChildren<Image>();

        
        if (instance == null)
        {
            instance = this;
        }

        HideText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (inDialogue)
            {
                NextDialogue();
            }
        }
    }

    private void HideText()
    {
        textbox_IMG.enabled = false;
        sprite_IMG.enabled = false;
        textbox.text = "";
    }

    private void ShowText()
    {
        textbox_IMG.enabled = true;
    }

    private void ShowSprite(Sprite sprite = null)
    {
        if (sprite != null)
        {
            sprite_IMG.enabled = true;
            sprite_IMG.sprite = sprite;
        }
        else
        {
            sprite_IMG.enabled = false;
        }
    }

    //Starts dialogue sequence if not in dialogue already
    public void StartDialogue(TextData dialogueList)
    {
        if (!inDialogue)
        {
            m_StartedDialogue.Invoke();
            ShowText();
            
            inDialogue = true;
            currentDialogueNumber = 0;
            thisDialogueList = dialogueList;

            ShowSprite(dialogueList.dialogue[0].image);

            StartCoroutine("Typewrite", dialogueList.dialogue[0].dialogueText);
        } 
    }

    //Triggers next dialogue in the sequence
    //If not done typing the current text, finish it immedietely
    //If done with the whole sequence, end the dialogue
    public void NextDialogue()
    {
        currentDialogueNumber++;

        if (currentDialogueNumber < thisDialogueList.dialogue.Length)
        {

            StartCoroutine("Typewrite", thisDialogueList.dialogue[currentDialogueNumber].dialogueText);

            ShowSprite(thisDialogueList.dialogue[currentDialogueNumber].image);
        }
        else
        {
            
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        inDialogue = false;

        HideText();
        m_EndedDialogue.Invoke();
    }

    //Types the text of the dialogue
    IEnumerator Typewrite(string text="No Text Here!")
    {
        visibleCharacters = 0;
        textbox.maxVisibleCharacters = 0;
        textbox.text = text;

        for (int i=0; i<=text.Length; i++)
        {
            textbox.maxVisibleCharacters = visibleCharacters;
            visibleCharacters+=charactersPerFrame;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        
        yield return null;
    }
}
