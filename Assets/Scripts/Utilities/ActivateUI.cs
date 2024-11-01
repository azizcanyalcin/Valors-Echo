using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> instructionsUI;
    [SerializeField] private List<KeyCode> instructionKeys;
    [SerializeField] private Dictionary<KeyCode, GameObject> instructionDictionary;
    private int currentInstructionIndex = 0;

    private void Start()
    {
        instructionDictionary = new Dictionary<KeyCode, GameObject>();

        for (int i = 0; i < instructionsUI.Count && i < instructionKeys.Count; i++)
        {
            instructionDictionary[instructionKeys[i]] = instructionsUI[i];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && instructionsUI.Count > 0)
        {
            ShowInstruction(instructionKeys[0]);
        }
    }

    private void Update()
    {
        foreach (var key in instructionDictionary.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                HideInstruction(key);

                if (currentInstructionIndex < instructionKeys.Count - 1)
                {
                    currentInstructionIndex++;
                    ShowInstruction(instructionKeys[currentInstructionIndex]);
                }
                break;
            }
        }
    }

    private void ShowInstruction(KeyCode key)
    {
        if (instructionDictionary.ContainsKey(key))
        {
            instructionDictionary[key].SetActive(true);
        }
    }

    private void HideInstruction(KeyCode key)
    {
        if (instructionDictionary.ContainsKey(key))
        {
            instructionDictionary[key].SetActive(false);
        }
    }
}
