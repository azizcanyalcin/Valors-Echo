using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    public List<InventoryItem> inventory;
    public Dictionary<Item, InventoryItem> inventoryDictionary;

    public Dictionary<Item, InventoryItem> stashDictionary;
    public List<InventoryItem> stash;

    public List<InventoryItem> equipment;
    public Dictionary<Equipment, InventoryItem> equipmentDictionary;

    public List<Item> startingEquipment;


    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;
    private ItemSlotUI[] inventoryItemSlot;
    private ItemSlotUI[] stashItemSlot;
    private EquipmentSlotUI[] equipmentSlot;
    private StatSlotUI[] statSlot;

    [Header("Item cooldown")]
    private float potionCooldownTimer;
    private float armorCooldownTimer;
    public float potionCooldown { get; private set; }
    private float armorCooldown;

    [Header("Database")]
    public List<Item> itemDatabase;
    public List<InventoryItem> loadedItems;
    public List<Equipment> loadedEquipments;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<Item, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<Item, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<ItemSlotUI>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<EquipmentSlotUI>();
        statSlot = statSlotParent.GetComponentsInChildren<StatSlotUI>();

        StartCoroutine(StartingItems());

    }
    IEnumerator StartingItems()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (Equipment equipment in loadedEquipments)
        {
            EquipItem(equipment);
        }

        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.itemData);
                }
            }
        }
        else
        {
            foreach (var item in startingEquipment)
            {
                if (startingEquipment != null)
                    AddItem(item);
            }
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanSlot();
        }
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanSlot();
        }


        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<Equipment, InventoryItem> equipment in equipmentDictionary)
            {
                if (equipment.Key.equipmentType == equipmentSlot[i].slotType)
                    equipmentSlot[i].UpdateSlot(equipment.Value);
            }
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueText();
        }
    }

    public void AddItem(Item item)
    {
        if (item.itemType == ItemType.Equipment && CanAddItem()) AddToInventory(item);
        else if (item.itemType == ItemType.Material) AddToStash(item);

        UpdateSlotUI();
    }
    public void RemoveItem(Item item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem inventoryValue))
        {
            if (inventoryValue.stackSize <= 1)
            {
                inventory.Remove(inventoryValue);
                inventoryDictionary.Remove(item);
            }
            else inventoryValue.RemoveStack();
        }

        if (stashDictionary.TryGetValue(item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(item);
            }
            else stashValue.RemoveStack();
        }
        UpdateSlotUI();
    }
    private void AddToInventory(Item item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value)) value.AddStack();

        else
        {
            InventoryItem newItem = new(item);
            inventory.Add(newItem);
            inventoryDictionary.Add(item, newItem);
        }
    }
    private void AddToStash(Item item)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem value)) value.AddStack();

        else
        {
            InventoryItem newItem = new(item);
            stash.Add(newItem);
            stashDictionary.Add(item, newItem);
        }
    }
    public void EquipItem(Item item)
    {
        Equipment newEquipment = item as Equipment;
        InventoryItem newItem = new(newEquipment);
        Equipment oldEquipment = null;

        foreach (KeyValuePair<Equipment, InventoryItem> equipment in equipmentDictionary)
        {
            if (equipment.Key.equipmentType == newEquipment.equipmentType) oldEquipment = equipment.Key;
        }

        if (oldEquipment != null)
        {
            UnEquipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();
        RemoveItem(item);
        UpdateSlotUI();
    }
    public void UnEquipItem(Equipment equipmentToRemove)
    {
        if (equipmentDictionary.TryGetValue(equipmentToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(equipmentToRemove);
            equipmentToRemove.RemoveModifiers();
        }
    }
    public bool CanCraftEquipment(Equipment equipmentToCraft, List<InventoryItem> requiredMaterials)
    {
        foreach (var material in requiredMaterials)
        {
            if (stashDictionary.TryGetValue(material.itemData, out InventoryItem stashItem))
            {
                if (stashItem.stackSize < material.stackSize)
                {
                    Debug.Log($"Not enough material.");
                    AudioManager.instance.PlaySFX(45, PlayerManager.instance.player.transform, false);
                    return false;

                }
            }
            else
            {
                Debug.Log($"No Material");
                AudioManager.instance.PlaySFX(45, PlayerManager.instance.player.transform, false);
                return false;

            }
        }

        foreach (var material in requiredMaterials)
        {
            for (int i = 0; i < material.stackSize; i++)
            {
                RemoveItem(material.itemData);
            }
        }

        AddItem(equipmentToCraft);
        Debug.Log($"Craft is successfull");
        AudioManager.instance.PlaySFX(44, PlayerManager.instance.player.transform, false);
        return true;
    }
    public bool CanAddItem()
    {
        if (inventory.Count >= inventoryItemSlot.Length)
        {
            Debug.LogError($"No More Space!");
            return false;
        }
        return true;
    }
    public List<InventoryItem> GetEquipments() => equipment;
    public List<InventoryItem> GetStashItems() => stash;

    public Equipment GetEquipment(EquipmentType type)
    {
        Equipment equipedItem = null;

        foreach (KeyValuePair<Equipment, InventoryItem> equipment in equipmentDictionary)
        {
            if (equipment.Key.equipmentType == type) equipedItem = equipment.Key;
        }
        return equipedItem;
    }

    public void UsePotion()
    {
        Equipment currentPotion = GetEquipment(EquipmentType.Potion);
        if (currentPotion == null)
        {
            Debug.LogError("No potion!");
            return;
        }

        bool canUsePotion = Time.time > potionCooldownTimer + potionCooldown;

        if (canUsePotion)
        {
            potionCooldown = currentPotion.itemCooldown;
            currentPotion.Effect(null);
            potionCooldownTimer = Time.time;
        }
        else Debug.Log($"Potion on cooldown");
    }
    public bool CanUseArmorEffect()
    {
        Equipment currentArmor = GetEquipment(EquipmentType.Armor);
        if (Time.time > armorCooldownTimer + armorCooldown)
        {
            potionCooldown = currentArmor.itemCooldown;
            armorCooldownTimer = Time.time;
            return true;
        }
        Debug.Log($"Armor is on cooldown");
        return false;
    }

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, int> pair in data.inventory)
        {
            foreach (var item in itemDatabase)
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new(item)
                    {
                        stackSize = pair.Value
                    };

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string equipmentId in data.equipments)
        {
            foreach (var equipment in itemDatabase)
            {
                if (equipment != null && equipmentId == equipment.itemId)
                {
                    loadedEquipments.Add(equipment as Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.inventory.Clear();
        data.equipments.Clear();
        
        foreach (KeyValuePair<Item, InventoryItem> pair in inventoryDictionary)
        {
            data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (KeyValuePair<Item, InventoryItem> pair in stashDictionary)
        {
            data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (KeyValuePair<Equipment, InventoryItem> pair in equipmentDictionary)
        {
            data.equipments.Add(pair.Key.itemId);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Load items")]
    private void LoadItemsToItemDatabase()
    {
        itemDatabase = new List<Item>(GetItemDatabase());
    }
    private List<Item> GetItemDatabase()
    {
        List<Item> itemDatabase = new();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var item = AssetDatabase.LoadAssetAtPath<Item>(SOpath);
            itemDatabase.Add(item);

        }
        return itemDatabase;
    }
#endif
}