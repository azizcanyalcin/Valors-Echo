using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] SceneTransition transition;

    private void Start()
    {
        continueButton.SetActive(SaveManager.instance.HasSavedData());
    }

    public void ContinueGame()
    {
        StartCoroutine(SceneTransition(1.3f));
    }

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