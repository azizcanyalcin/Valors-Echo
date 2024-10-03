using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class UIManager : MonoBehaviour, ISaveManager
{
    public SceneTransitionUI transition;
    [SerializeField] GameObject restartButton;
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
        Switch(skillTreeUI);
        skillTreeUI.SetActive(true);
        transition.gameObject.SetActive(true);
    }
    private void Start()
    {
        Switch(inGameUI);
        skillTreeUI.SetActive(false);
        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKey(characterUI);

        if (Input.GetKeyDown(KeyCode.V))
            SwitchWithKey(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKey(craftUI);

        if (Input.GetKeyDown(KeyCode.Escape))
            SwitchWithKey(optionsUI);

    }
    public void Switch(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<SceneTransitionUI>()) break;
            transform.GetChild(i).gameObject.SetActive(false);
        }
        if (menu != null) menu.SetActive(true);

        if (GameManager.instance)
        {
            if (menu == optionsUI || menu == craftUI) GameManager.instance.PauseGame(true);
            else GameManager.instance.PauseGame(false);
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
        //GameManager.instance.PauseGame(true);
        Switch(inGameUI);
    }

    public void SwitchToEndScreen()
    {
        transition.FadeOut();
        StartCoroutine(EndScreen());
    }
    IEnumerator EndScreen()
    {
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }
    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, float> pair in data.volumeSettings)
        {
            foreach (VolumeSettingsUI setting in volumeSettings)
            {
                if (setting.param == pair.Key) setting.LoadSlider(pair.Value);
            }
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