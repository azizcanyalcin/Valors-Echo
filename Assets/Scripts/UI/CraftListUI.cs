using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftListUI : MonoBehaviour
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;
    [SerializeField] private List<Equipment> craftEquipments;


    private void Start()
    {
        transform.parent.GetChild(0).GetComponent<CraftListUI>().SetupCraftList();
        SetupDefaultCraftWindow();
    }



    public void SetupCraftList()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        foreach (var craftEquipment in craftEquipments)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<CraftSlotUI>().SetupCraftSlot(craftEquipment);
        }
    }

    public void SetupDefaultCraftWindow()
    {
        if (craftEquipments[0] != null)
            GetComponentInParent<UIManager>().craftWindowUI.SetupCraftWindow(craftEquipments[0]);
    }
}