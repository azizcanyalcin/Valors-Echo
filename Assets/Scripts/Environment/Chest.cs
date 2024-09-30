using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool isOpened = false;
    private ItemDrop itemDrop;
    private Animator chestAnimator;
    void Start()
    {
        chestAnimator = GetComponent<Animator>();
        itemDrop = GetComponent<ItemDrop>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }
    private void OpenChest()
    {
        isOpened = true;
        chestAnimator.SetTrigger("Open"); 
        itemDrop.GenerateDrop();
    }
}
