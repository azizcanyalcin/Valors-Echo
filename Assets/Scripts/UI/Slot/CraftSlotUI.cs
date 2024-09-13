using UnityEngine;
using UnityEngine.EventSystems;

public class CraftSlotUI : ItemSlotUI
{
    protected override void Start()
    {
        base.Start();
    }
    public void SetupCraftSlot(Equipment equipment)
    {
        if(equipment == null) return;
        
        item.itemData = equipment;
        itemImage.sprite = equipment.icon;
        itemText.text = equipment.itemName;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        uiManager.craftWindowUI.SetupCraftWindow(item.itemData as Equipment);
    }
}