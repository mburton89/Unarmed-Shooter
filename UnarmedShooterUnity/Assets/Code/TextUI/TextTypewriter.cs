using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextTypewriter : MonoBehaviour
{

    TextMeshProUGUI textbox;

    int visibleCharacters = 0;
    int currentDialogueNumber = 0;
    bool isDoneTyping = true;
    bool inDialogue = false;

    TextData thisDialogueList;

    [SerializeField] int charactersPerFrame = 2;

    private void Awake()
    {
        textbox = GetComponent<TextMeshProUGUI>();
    }

    //TODO: Add StartDialogue trigger
    //Starts dialogue sequence if not in dialogue already
    public void StartDialogue(TextData dialogueList)
    {
        if (!inDialogue)
        {
            inDialogue = true;
            currentDialogueNumber = 0;
            thisDialogueList = dialogueList;
            StartCoroutine("Typewrite", dialogueList.dialogue[0].dialogueText);
        } 
    }

    //Triggers next dialogue in the sequence
    //If not done typing the current text, finish it immedietely
    //If done with the whole sequence, end the dialogue
    //TODO: Add EndDialogue trigger
    public void NextDialogue()
    {
        currentDialogueNumber++;

        if (currentDialogueNumber < thisDialogueList.dialogue.Length)
        {
            if (!isDoneTyping)
            {
                StopAllCoroutines();
                textbox.maxVisibleCharacters = textbox.text.Length;
                isDoneTyping = true;
            }
            else
            {
                StartCoroutine("Typewrite", thisDialogueList.dialogue[currentDialogueNumber].dialogueText);
            }
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        isDoneTyping = true; //Failsafe
        inDialogue = false;
    }

    //Types the text of the dialogue
    IEnumerator Typewrite(string text="No Text Here!")
    {
        visibleCharacters = 0;
        textbox.maxVisibleCharacters = 0;
        textbox.text = text;
        isDoneTyping = false;

        for (int i=0; i<=text.Length; i++)
        {
            textbox.maxVisibleCharacters = visibleCharacters;
            visibleCharacters+=charactersPerFrame;
            yield return new WaitForSeconds(0.01f);
        }

        isDoneTyping = true;
        yield return null;
    }
}
