using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class UIManager : MonoBehaviour, ISaveManager
{
    public SceneTransitionUI transition;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private VolumeSettingsUI[] volumeSettings;

    [Space]
    [Header("Menus")]
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;

    [Header("ToolTips")]
    public ItemToolTipUI itemToolTip;
    public StatToolTipUI statToolTip;
    public SkillToolTipUI skillToolTip;
    public CraftWindowUI craftWindowUI;

    private void Awake()
    {
        skillTreeUI.SetActive(true);
        characterUI.SetActive(true);
        inGameUI.SetActive(true);
        transition.gameObject.SetActive(true);
    }

    private void Start()
    {
        Switch(inGameUI);
        skillTreeUI.SetActive(false);
        characterUI.SetActive(false);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        skillToolTip.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKey(characterUI);

        if (Input.GetKeyDown(KeyCode.V))
            SwitchWithKey(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.O))
            SwitchWithKey(optionsUI);

        // Close all menus except InGameUI with "Esc"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllMenusExceptInGame();
        }
    }

    public void Switch(GameObject menu)
    {
        GameObject activeMenu = GetActiveMenu();  // Get the currently active menu

        // If there's an active menu that's not the same as the new one, deactivate it
        if (activeMenu != null && activeMenu != menu)
            activeMenu.SetActive(false);

        // Activate the desired menu
        if (menu != null)
            menu.SetActive(true);

        // Handling game pause logic
        if (GameManager.instance != null)
        {
            if (menu == characterUI || menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
        }
    }

    public void SwitchWithKey(GameObject menu)
    {
        if (menu != null && menu.activeSelf)  // If the menu is already active, deactivate it
        {
            menu.SetActive(false);
            SwitchToInGameUI();  // Switch back to in-game UI when a menu is closed
            return;
        }

        Switch(menu);
    }

    private void SwitchToInGameUI()
    {
        // Ensure no other UI (except SceneTransitionUI) is active before switching to inGameUI
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<SceneTransitionUI>() == null)
                return;
        }

        // Only switch if inGameUI is not already active
        if (!inGameUI.activeSelf)
            Switch(inGameUI);
    }
    private void CloseAllMenusExceptInGame()
    {
        // Loop through all the child objects (menus) and deactivate them, except for the inGameUI and SceneTransitionUI
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            // Ignore InGameUI and SceneTransitionUI, close all others
            if (child != inGameUI && child.GetComponent<SceneTransitionUI>() == null)
            {
                child.SetActive(false);
            }
        }

        // Ensure the inGameUI is activated after closing all other menus
        if (!inGameUI.activeSelf)
        {
            inGameUI.SetActive(true);
            GameManager.instance.PauseGame(false);
        }
    }

    private GameObject GetActiveMenu()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf && child.GetComponent<SceneTransitionUI>() == null)
                return child.gameObject;
        }
        return null;
    }

    public void SwitchToEndScreen()
    {
        if (transition != null)
        {
            transition.FadeOut();
            StartCoroutine(EndScreen());
        }
        else
        {
            restartButton.SetActive(true);  // Fallback if transition is not available
        }
    }

    IEnumerator EndScreen()
    {
        float fadeDuration = transition != null ? transition.fadeOutDuration : 1.5f;
        yield return new WaitForSeconds(fadeDuration);
        restartButton.SetActive(true);
    }

    public void RestartGameButton()
    {
        if (GameManager.instance != null)
            GameManager.instance.RestartScene();
    }

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, float> pair in data.volumeSettings)
        {
            VolumeSettingsUI matchingSetting = Array.Find(volumeSettings, setting => setting.param == pair.Key);
            if (matchingSetting != null)
                matchingSetting.LoadSlider(pair.Value);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.volumeSettings.Clear();

        foreach (VolumeSettingsUI setting in volumeSettings)
        {
            data.volumeSettings.Add(setting.param, setting.slider.value);
        }
    }
}
