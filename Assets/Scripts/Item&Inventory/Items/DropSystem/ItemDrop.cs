using System.Collections.Generic;
using System.Linq;
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

        Vector2 velocity = new(Random.Range(-4, 4), Random.Range(8, 12));

        newDrop.GetComponent<ItemObject>().SetupItem(itemData, velocity);
    }

    public virtual void GenerateDrop()
    {
        if (possibleDrops.Length == 0)
        {
            Debug.Log($"item pool is empty");
            return;
        }

        foreach (Item item in possibleDrops)
        {
            if (item && Random.Range(0, 100) <= item.dropChance) dropList.Add(item);
        }

        for (int i = 0; i < dropAmount; i++)
        {
            if (dropList.Count > 0)
            {
                Item drop = dropList[Random.Range(0, dropList.Count - 1)];

                DropItem(drop);
                dropList.Remove(drop);
            }
        }
    }
}