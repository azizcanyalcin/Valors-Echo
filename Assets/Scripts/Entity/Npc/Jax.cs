using DialogueEditor;
using UnityEngine;

public class Jax : InteractableObject
{
    public NPCConversation jaxDialogue;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HandleConversationStart();
            HandleConversationEnd();
        }
    }

    private void HandleConversationStart()
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
