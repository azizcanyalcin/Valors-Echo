using UnityEngine;

public class Blacksmith : InteractableObject
{
    [SerializeField] private GameObject craftUI;
    private Player player;
    private bool isInTriggerZone = false;

    protected override void Start()
    {
        base.Start();
        player = PlayerManager.instance.player;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = false;
            CloseCraftPanel();
            keyE.enabled = false;
        }
    }

    private void Update()
    {
        if (isInTriggerZone && Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpened)
                OpenCraftPanel();
            else
                CloseCraftPanel();
        }
    }

    private void CloseCraftPanel()
    {
        craftUI.SetActive(false);
        isOpened = false;
        keyE.enabled = true;
        GameManager.instance.PauseGame(false);
    }

    protected virtual void OpenCraftPanel()
    {
        craftUI.SetActive(true);
        keyE.enabled = false;
        isOpened = true;
        player.SetVelocityToZero();
        GameManager.instance.PauseGame(true);
    }
}
