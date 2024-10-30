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
        PlayerManager.instance.player.isPlayerActive = false;
        PlayerManager.instance.player.SetVelocityToZero();
        transition.FadeOut();

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
        PlayerManager.instance.player.isPlayerActive = true;
        SaveManager.instance.LoadGame();
    }
    public void StartSceneTransition()
    {
        StartCoroutine(SceneTransition(2));
    }
}