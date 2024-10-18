using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionAuto : SceneTransitionHandler
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) StartCoroutine(SceneTransition(1.5f));
    }
    
}