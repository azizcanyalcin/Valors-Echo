using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        SetInitialMenuStates();
    }

    private void Start()
    {
        InitializeUI();
    }

    private void Update()
    {
        HandleInput();
    }

    private void SetInitialMenuStates()
    {
        skillTreeUI.SetActive(true);
        characterUI.SetActive(true);
        inGameUI.SetActive(true);
        transition.gameObject.SetActive(true);
    }

    private void InitializeUI()
    {
        Switch(inGameUI);
        skillTreeUI.SetActive(false);
        characterUI.SetActive(false);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        skillToolTip.gameObject.SetActive(false);
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.C)) ToggleMenu(characterUI);
        if (Input.GetKeyDown(KeyCode.V)) ToggleMenu(skillTreeUI);
        if (Input.GetKeyDown(KeyCode.O)) ToggleMenu(optionsUI);
        if (Input.GetKeyDown(KeyCode.H)) ToggleMenu(instructions);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GetActiveMenu() == inGameUI)
                ToggleMenu(inGameMenu);
            else
                CloseAllMenusExceptInGame();
        }
    }

    public void Switch(GameObject menu)
    {
        var activeMenu = GetActiveMenu();
        if (activeMenu != null && activeMenu != menu)
            activeMenu.SetActive(false);

        menu?.SetActive(true);

        GameManager.instance?.PauseGame(menu != inGameUI);
    }

    public void ToggleMenu(GameObject menu)
    {
        if (menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            SwitchToInGameUI();
        }
        else
        {
            Switch(menu);
        }
    }

    private void SwitchToInGameUI()
    {
        if (transform.Cast<Transform>().All(child => !child.gameObject.activeSelf || child.GetComponent<SceneTransitionUI>() != null))
        {
            if (!inGameUI.activeSelf)
                Switch(inGameUI);
        }
    }

    private void CloseAllMenusExceptInGame()
    {
        foreach (Transform child in transform)
        {
            var childGameObject = child.gameObject;
            if (childGameObject != inGameUI && childGameObject.GetComponent<SceneTransitionUI>() == null)
            {
                childGameObject.SetActive(false);
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
        return transform.Cast<Transform>()
                        .FirstOrDefault(child => child.gameObject.activeSelf && child.GetComponent<SceneTransitionUI>() == null)
                        ?.gameObject;
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

    private IEnumerator EndScreen()
    {
        float fadeDuration = transition != null ? transition.fadeOutDuration : 1.5f;
        yield return new WaitForSeconds(fadeDuration);
        restartButton.SetActive(true);
    }

    public void RestartGameButton()
    {
        GameManager.instance?.RestartScene();
    }

    public void LoadData(GameData data)
    {
        foreach (var pair in data.volumeSettings)
        {
            var matchingSetting = volumeSettings.FirstOrDefault(setting => setting.param == pair.Key);
            matchingSetting?.LoadSlider(pair.Value);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.volumeSettings.Clear();
        foreach (var setting in volumeSettings)
        {
            data.volumeSettings.Add(setting.param, setting.slider.value);
        }
    }
}