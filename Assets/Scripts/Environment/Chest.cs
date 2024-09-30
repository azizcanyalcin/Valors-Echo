using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public bool isOpened = false;
    private ItemDrop itemDrop;
    private Animator chestAnimator;
    private bool canKeyEnable = true;
    [SerializeField] private Image keyE;
    void Start()
    {
        chestAnimator = GetComponent<Animator>();
        itemDrop = GetComponent<ItemDrop>();
        keyE.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(canKeyEnable)
                keyE.enabled = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            keyE.enabled = false;
        }
    }
    private void OpenChest()
    {
        keyE.enabled = false;
        canKeyEnable = false;
        isOpened = true;
        chestAnimator.SetTrigger("Open");
        itemDrop.GenerateDrop();
    }
}
