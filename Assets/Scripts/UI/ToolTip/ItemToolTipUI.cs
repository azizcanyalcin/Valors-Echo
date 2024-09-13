using TMPro;
using UnityEngine;

public class ItemToolTipUI : ToolTipManager
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void ShowItemTooltip(Equipment item)
    {
        if (item == null) return;
        itemName.text = item.itemName;
        itemType.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();

        AdjustPosition();

        gameObject.SetActive(true);
    }
    public void HideItemTooltip() => gameObject.SetActive(false);
}