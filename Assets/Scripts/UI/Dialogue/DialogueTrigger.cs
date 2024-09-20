using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string tagName;
    public Dialogue dialogue;
    public bool isTriggered = false;    
    public void TriggerDialogue()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagName))
        {
            TriggerDialogue();
        }
    }
}
