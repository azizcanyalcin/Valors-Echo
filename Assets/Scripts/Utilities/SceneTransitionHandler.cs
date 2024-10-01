using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHandler : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] SceneTransitionUI transition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) StartCoroutine(SceneTransition(1.5f));
    }
    IEnumerator SceneTransition(float delay)
    {
        SaveManager.instance.SaveGame();
        transition.FadeOut();

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }
}