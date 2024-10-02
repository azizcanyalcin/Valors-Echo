using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public bool isOpened = false;
    private Animator animator;
    private bool canKeyEnable = true;
    [SerializeField] private Image keyE;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        keyE.enabled = false;
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (canKeyEnable)
                keyE.enabled = true;
        }
    }
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            OpenObject();
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            keyE.enabled = false;
        }
    }
    protected virtual void OpenObject()
    {
        keyE.enabled = false;
        canKeyEnable = false;
        isOpened = true;
        animator.SetTrigger("Open");
    }
}