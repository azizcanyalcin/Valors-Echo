using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Animator animator;
    public string id;
    public bool isActive;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    [ContextMenu("Generate Checkpoint Id")]
    private void GenerateId()
    {
        id = Guid.NewGuid().ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>()) ActivateCheckpoints();
    }

    public void ActivateCheckpoints()
    {
        if (isActive) return;
        isActive = true;
        animator.SetBool("active", true);
        AudioManager.instance.PlaySFX(5, transform, true);
    }
}