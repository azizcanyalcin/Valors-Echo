using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image image;
    public float delay;
    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        image = GetComponentInChildren<Image>();
        image.color = Color.clear;
    }
    public void StartDialogue(Dialogue dialogue)
    {
        image.color = new Color(236f / 255f, 202f / 255f, 154f / 255f);
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, delay));
    }

    IEnumerator TypeSentence(string sentence, float delay)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(delay * 4);
        DisplayNextSentence();
    }

    public void EndDialogue()
    {
        image.color = Color.clear;
        dialogueText.text = "";
    }
}
