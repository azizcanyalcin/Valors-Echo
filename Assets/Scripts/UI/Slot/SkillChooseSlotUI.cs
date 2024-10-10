using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillChooseSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private UIManager uiManager;
    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    public bool unlocked;
    [SerializeField] private SkillTreeSlotUI[] shouldBeUnlocked;
    [SerializeField] private SkillTreeSlotUI[] shouldBeLocked;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlotUI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }
    private void Start()
    {
        uiManager = GetComponentInParent<UIManager>();
    }

    public void UnlockSkillSlot()
    {
        unlocked = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiManager.skillToolTip.ShowSkillToolTip(skillDescription, skillName, skillCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiManager.skillToolTip.HideSkillToolTip();
    }

    public void LoadData(GameData data)
    {
        if(data.skillTree.TryGetValue(skillName, out bool value)) unlocked = value;
    }

    public void SaveData(ref GameData data)
    {
        if (data.skillTree.TryGetValue(skillName, out bool value))
        {
            data.skillTree.Remove(skillName);
            data.skillTree.Add(skillName, unlocked);
        }
        else data.skillTree.Add(skillName, unlocked);
    }
}