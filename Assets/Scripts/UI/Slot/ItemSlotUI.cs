using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;
    [SerializeField] protected Image borderImage; // Add this line
    protected UIManager uiManager;
    public InventoryItem item;

    protected virtual void Start()
    {
        uiManager = GetComponentInParent<UIManager>();
        if (borderImage != null)
        {
            borderImage.enabled = false; // Ensure the border is initially hidden
        }
    }

    public void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;

        if (item != null)
        {
            itemImage.sprite = item.itemData.icon;
            itemImage.color = Color.white;
            if (item.stackSize > 1) itemText.text = item.stackSize.ToString();
            else itemText.text = "";
        }
        else
        {
            CleanSlot();
        }
    }

    public void CleanSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftControl) && item != null)
        {
            Inventory.instance.RemoveItem(item.itemData);
            return;
        }

        if (item.itemData.itemType == ItemType.Equipment && item != null)
        {
            Debug.Log($"{item.itemData.itemName} is equipped!");
            Inventory.instance.EquipItem(item.itemData);
        }
        uiManager.itemToolTip.HideItemTooltip();
        AudioManager.instance.PlaySFX(64);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (borderImage != null)
        {
            borderImage.enabled = true; // Show the border
        }
        if (item == null) return;
        uiManager.itemToolTip.ShowItemTooltip(item.itemData as Equipment);
        AudioManager.instance.PlaySFX(46);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (borderImage != null)
        {
            borderImage.enabled = false; // Hide the border
        }
        if (item != null)
        {
            itemImage.color = Color.white;
        }
        else
        {
            itemImage.color = Color.clear;
        }
        if (item == null) return;
        uiManager.itemToolTip.HideItemTooltip();
    }
}

