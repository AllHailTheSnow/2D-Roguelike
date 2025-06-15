using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotUI : ItemSlotUI
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(item == null || item.itemData == null) { return; }

        Inventory.Instance.UnequipItem(item.itemData as ItemDataEquipment);
        Inventory.Instance.AddItem(item.itemData as ItemDataEquipment);
        CleanupSlot();
        uiController.toolTip.HideToolTip();
    }
}
