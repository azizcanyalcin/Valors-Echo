using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHandler : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] protected SceneTransitionUI transition;

    public virtual IEnumerator SceneTransition(float delay)
    {
        PlayerManager.instance.player.isPlayerActive = false;
        PlayerManager.instance.player.SetVelocityToZero();
        SaveManager.instance.SaveGame();
        transition.FadeOut();

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
        PlayerManager.instance.player.isPlayerActive = true;
    }
}