using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [SerializeField] TextData dialogue;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerShip>() != null)
        {
            TextTypewriter.instance.StartDialogue(dialogue);
            gameObject.SetActive(false);
        }
    }
}
