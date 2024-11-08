using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneName;
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
    public void PlayButtonClickSFX()
    {
        AudioManager.instance.PlaySFX(47);
    }
    public void PlayButtonHoverSFX()
    {
        AudioManager.instance.PlaySFX(46);
    }
    IEnumerator SceneTransition(float delay) 
    {
        transition.FadeOut();

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }

}