using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftWindowUI : MonoBehaviour
{
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected TextMeshProUGUI itemName;
    [SerializeField] protected TextMeshProUGUI itemDescription;
    [SerializeField] private Image[] materialSlots;
    [SerializeField] private Button craftButton;

    public void SetupCraftWindow(Equipment equipment)
    {
        craftButton.onClick.RemoveAllListeners();

        foreach (var materialImage in materialSlots)
        {
            materialImage.color = Color.clear;
            materialImage.GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }
        for (int i = 0; i < equipment.craftMaterials.Count; i++)
        {
            materialSlots[i].sprite = equipment.craftMaterials[i].itemData.icon;
            materialSlots[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialSlots[i].GetComponentInChildren<TextMeshProUGUI>();

            materialSlotText.text = equipment.craftMaterials[i].stackSize.ToString();
            materialSlotText.color = Color.white;
        }
        
        itemIcon.sprite = equipment.icon;
        itemName.text = equipment.itemName;
        itemDescription.text = equipment.GetDescription();
        
        craftButton.onClick.AddListener(() => Inventory.instance.CanCraftEquipment(equipment, equipment.craftMaterials));
    }
}