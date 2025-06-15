using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftListUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftslotParent;
    [SerializeField] private GameObject craftSlotPrefab;
    [SerializeField] private List<ItemDataEquipment> craftEquipment;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent.GetChild(0).GetComponent<CraftListUI>().SetupCraftList();
        SetupDefaultCraftWindow();
    }

    public void SetupCraftList()
    {
        for (int i = 0; i < craftslotParent.childCount; i++)
        {
            Destroy(craftslotParent.GetChild(i).gameObject);
        }


        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newCraftSlot = Instantiate(craftSlotPrefab, craftslotParent);
            newCraftSlot.GetComponent<CraftSlotUI>().SetupCraftSlot(craftEquipment[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }

    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
        {
            GetComponentInParent<UIController>().craftWindowUI.SetupCraftWindow(craftEquipment[0]);
        }
    }
}
