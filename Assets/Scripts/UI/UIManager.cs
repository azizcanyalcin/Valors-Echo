using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject inGameMenu;

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

        if (Input.GetKeyDown(KeyCode.H))
            SwitchWithKey(instructions);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllMenusExceptInGame();
        }
        if (GetActiveMenu() == inGameUI && Input.GetKeyDown(KeyCode.Escape))
            SwitchWithKey(inGameMenu);
    }
    public void Switch(GameObject menu)
    {
        GameObject activeMenu = GetActiveMenu();
        if (activeMenu != null && activeMenu != menu)
            activeMenu.SetActive(false);

        if (menu != null)
            menu.SetActive(true);

        if (GameManager.instance != null)
        {
            if (menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
        }
    }
    public void SwitchWithKey(GameObject menu)
    {
        if (menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            SwitchToInGameUI();
            return;
        }

        Switch(menu);
    }
    private void SwitchToInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<SceneTransitionUI>() == null)
                return;
        }

        if (!inGameUI.activeSelf)
            Switch(inGameUI);
    }
    private void CloseAllMenusExceptInGame()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (child != inGameUI && child.GetComponent<SceneTransitionUI>() == null)
            {
                child.SetActive(false);
            }
        }

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
            restartButton.SetActive(true);
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

