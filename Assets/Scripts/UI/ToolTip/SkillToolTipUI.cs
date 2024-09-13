using TMPro;
using UnityEngine;

public class SkillToolTipUI : ToolTipManager
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillCost;

    public void ShowSkillToolTip(string skillDescription, string skillName, int skillCost )
    {
        this.skillDescription.text = skillDescription;
        this.skillName.text = skillName;
        this.skillCost.text = skillCost.ToString();

        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideSkillToolTip()
    {   
        gameObject.SetActive(false);
    }
}