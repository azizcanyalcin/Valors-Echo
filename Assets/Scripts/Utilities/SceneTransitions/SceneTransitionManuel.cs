using System.Collections;
using UnityEngine;

public class SceneTransitionManuel : InteractableObject
{
    [SerializeField] private SceneTransitionHandler transitionHandler;
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            transitionHandler.StartCoroutine(transitionHandler.SceneTransition(1.5f));
        }
    }
    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }
}