using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private Item[] possibleDrops;
    [SerializeField] private int dropAmount;
    private List<Item> dropList = new();

    protected void DropItem(Item itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 velocity = new(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(itemData, velocity);
    }

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrops.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrops[i].dropChance) dropList.Add(possibleDrops[i]);
        }
        for (int i = 0; i < dropAmount; i++)
        {
            Item drop = dropList[Random.Range(0, dropList.Count - 1)];

            dropList.Remove(drop);
            DropItem(drop);
        }

    }
}