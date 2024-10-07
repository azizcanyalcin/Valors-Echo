using UnityEngine;

public class AnimatedObject : InteractableObject
{
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            OpenObject();
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