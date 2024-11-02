using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;
    protected UIManager uiManager;
    public InventoryItem item;
    protected virtual void Start()
    {
        uiManager = GetComponentInParent<UIManager>();
    }
    public void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;
        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.itemData.icon;
            if (item.stackSize > 1) itemText.text = item.stackSize.ToString();
            else itemText.text = "";
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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;
        uiManager.itemToolTip.ShowItemTooltip(item.itemData as Equipment);
        AudioManager.instance.PlaySFX(46);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) return;
        uiManager.itemToolTip.HideItemTooltip();
    }
}