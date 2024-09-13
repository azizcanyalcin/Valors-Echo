using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's Drop")]
    [SerializeField] private float equipmentDropChance;
    [SerializeField] private float materialDropChance;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        List<InventoryItem> equipmentsToDrop = new();
        List<InventoryItem> materialsToDrop = new();

        DropItem(inventory, inventory.GetEquipments(), equipmentsToDrop, equipmentDropChance);
        DropItem(inventory, inventory.GetStashItems(), materialsToDrop, materialDropChance);

    }

    private void DropItem(Inventory inventory, List<InventoryItem> currentEquipment, List<InventoryItem> itemsToDrop, float dropChance)
    {
        foreach (InventoryItem item in currentEquipment)
        {
            if (Random.Range(0, 100) <= dropChance)
            {
                DropItem(item.itemData);
                itemsToDrop.Add(item);
            }
        }

        foreach (var item in itemsToDrop)
        {
            if (item.itemData is Equipment equipment)
            {
                inventory.UnEquipItem(equipment);
            }
            else
            {
                inventory.RemoveItem(item.itemData);
            }
        }
    }

}