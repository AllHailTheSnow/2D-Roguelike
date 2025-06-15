using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player Item Drop")]
    [SerializeField] private float chanceToDropEquipment;
    [SerializeField] private float chanceToDropItems;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.Instance;

        List<InventoryItem> itemsToUnequip = new();
        List<InventoryItem> itemsToDrop = new();

        //loop through the current equipment and drop items based on the chance to drop items
        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= chanceToDropEquipment)
            {
                DropItem(item.itemData);
                itemsToUnequip.Add(item);
            }
        }

        //loop through the current stash and drop items based on the chance to drop items
        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToDropItems)
            {
                DropItem(item.itemData);
                itemsToDrop.Add(item);
            }
        }

        //unequip the items that were dropped
        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].itemData as ItemDataEquipment);
        }

        //remove the items that were dropped from the inventory
        for (int i = 0; i < itemsToDrop.Count; i++)
        {
            inventory.RemoveItem(itemsToDrop[i].itemData);
        }
    }
}
