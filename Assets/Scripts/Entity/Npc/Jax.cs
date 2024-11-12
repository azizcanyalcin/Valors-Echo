using DialogueEditor;
using UnityEngine;

public class Jax : InteractableObject
{
    public NPCConversation jaxDialogueLose;
    public NPCConversation jaxDialogueWin;
    private CountdownManager countdownManager;
    private bool isWin = true;

    protected override void Start()
    {
        base.Start();
        countdownManager = FindObjectOfType<CountdownManager>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (countdownManager.countdownTime > 0) isWin = true;
        else isWin = false;
        Debug.Log("Is Win: " + isWin);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isWin)
            {
                HandleConversationStart(jaxDialogueWin);
                countdownManager.StopCountdown();
            }
            else if (!isWin)
                HandleConversationStart(jaxDialogueLose);
            HandleConversationEnd();
        }
    }

    private void HandleConversationStart(NPCConversation jaxDialogue)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ConversationManager.Instance != null && jaxDialogue != null)
            {
                ConversationManager.Instance.StartConversation(jaxDialogue);
                keyE.enabled = false;

                if (PlayerManager.instance != null)
                    PlayerManager.instance.player.isPlayerActive = false;
            }
            else
            {
                Debug.LogWarning("ConversationManager or jaxDialogue is not assigned.");
            }
        }
    }

    private void HandleConversationEnd()
    {
        if (ConversationManager.Instance != null && (!ConversationManager.Instance.IsConversationActive || Input.GetKeyDown(KeyCode.Escape)))
        {
            if (PlayerManager.instance != null)
                PlayerManager.instance.player.isPlayerActive = true;
        }
    }
}
