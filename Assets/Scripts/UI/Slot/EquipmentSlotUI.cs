using UnityEngine;
using UnityEngine.EventSystems;
public class EquipmentSlotUI : ItemSlotUI
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.itemData == null) return;

        Inventory.instance.UnEquipItem(item.itemData as Equipment);
        Inventory.instance.AddItem(item.itemData as Equipment);

        uiManager.itemToolTip.HideItemTooltip();

        CleanSlot();
    }
}