using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneName = "Level0";
    [SerializeField] private GameObject continueButton;
    [SerializeField] SceneTransitionUI transition;
    public void NewGame()
    {
        SaveManager.instance.DeleteDatabase();
        StartCoroutine(SceneTransition(1.5f));
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    IEnumerator SceneTransition(float delay) 
    {
        transition.FadeOut();

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }
}