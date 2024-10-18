using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHandler : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] protected SceneTransitionUI transition;

    public virtual IEnumerator SceneTransition(float delay)
    {
        SaveManager.instance.SaveGame();
        transition.FadeOut();

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }
}