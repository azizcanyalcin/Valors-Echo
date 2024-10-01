using UnityEngine;

public class SceneTransitionUI : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeOut() => animator.SetTrigger("fadeOut");
    public void FadeIn() => animator.SetTrigger("fadeIn");
    
}