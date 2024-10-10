using UnityEngine;

public class SceneTransitionUI : MonoBehaviour
{
    private Animator animator;
    public float fadeOutDuration = 1.5f;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeOut() => animator.SetTrigger("fadeOut");
    public void FadeIn() => animator.SetTrigger("fadeIn");
    
}