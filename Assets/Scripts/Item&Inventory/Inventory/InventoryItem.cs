using System;

[Serializable]
public class InventoryItem
{
    public Item itemData;
    public int stackSize;
    public InventoryItem(Item newItemData)
    {
        itemData = newItemData;
        AddStack();

    }
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}