using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    Animator animator;
    public string id;
    public bool isActive;
    private Light2D spotLight;
    private void Start()
    {
        animator = GetComponent<Animator>();
        spotLight = GetComponentInChildren<Light2D>();
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
        spotLight.intensity = 2.7f;
        isActive = true;
        if (animator)
            animator.SetBool("active", true);
        AudioManager.instance.PlaySFX(5, transform, true);
    }
}