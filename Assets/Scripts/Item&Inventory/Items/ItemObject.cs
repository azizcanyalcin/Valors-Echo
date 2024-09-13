using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Item itemData;
    [SerializeField] private Rigidbody2D rb;

    private void SetupVisuals()
    {
        if (itemData == null) return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object - " + itemData.itemName;
    }

    private void Update()
    {

    }

    public void SetupItem(Item itemData, Vector2 velocity)
    {
        this.itemData = itemData;
        rb.velocity = velocity;
        SetupVisuals();
    }

    public void PickUpItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7);
            return;
        }
        Inventory.instance.AddItem(itemData);
        AudioManager.instance.PlaySFX(18, transform,false);
        Destroy(gameObject);
    }
}