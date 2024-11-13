using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHandler : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] protected SceneTransitionUI transition;

    public virtual async Task SceneTransition(float delay)
    {
        await SaveManager.instance.SaveGame();
        PlayerManager.instance.player.isPlayerActive = false;
        PlayerManager.instance.player.SetVelocityToZero();
        transition.FadeOut();

        await Task.Delay((int)(delay * 1000));

        SceneManager.LoadScene(sceneName);
        PlayerManager.instance.player.isPlayerActive = true;
        await SaveManager.instance.LoadGame();
    }

    public void StartSceneTransition()
    {
        StartCoroutine(SceneTransitionCoroutine(2));
    }

    private IEnumerator SceneTransitionCoroutine(float delay)
    {
        var task = SceneTransition(delay);
        while (!task.IsCompleted)
        {
            yield return null;
        }
    }
}
