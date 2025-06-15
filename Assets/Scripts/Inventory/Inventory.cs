using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : Singleton<Inventory>, ISaveManager
{
    public List<ItemData> startingEquipment;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> equipment;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private ItemSlotUI[] inventoryItemSlots;
    private ItemSlotUI[] stashItemSlots;
    private EquipmentSlotUI[] equipmentItemSlots;
    private StatSlotUI[] statSlots;

    [Header("Item Cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUseArmour;

    [Header("Item Database")]
    public List<ItemData> itemDatabase;
    public List<InventoryItem> loadedItems;
    public List<ItemDataEquipment> loadedEquipment;

    public float flaskCooldown { get; private set; }
    private float ArmourCooldown;

    override protected void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // Initialize the inventory items list and dictionary
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        // Initialize the stash items list and dictionary
        stashItems = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        //Initialize the equipment list and dictionary
        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();

        // Get the item slot UI components for the inventory and stash slots and equipment slots
        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
        stashItemSlots = stashSlotParent.GetComponentsInChildren<ItemSlotUI>();
        equipmentItemSlots = equipmentSlotParent.GetComponentsInChildren<EquipmentSlotUI>();
        statSlots = statSlotParent.GetComponentsInChildren<StatSlotUI>();

        AddStartingEquipment();
    }

    private void AddStartingEquipment()
    {
        // Check if there are any loaded items to add to the equipment
        foreach (ItemDataEquipment item in loadedEquipment)
        {
            //equip the item
            EquipItem(item);
        }

        // Check if there are any loaded items to add to the inventory
        if (loadedItems.Count > 0)
        {
            //for each item in the loaded items list, add it to the inventory
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.itemData);
                }
            }

            return;
        }

        // Add the starting equipment to the inventory
        for (int i = 0; i < startingEquipment.Count; i++)
        {
            if (startingEquipment[i] != null)
            {
                AddItem(startingEquipment[i]);
            }
        }
    }

    private void UpdateUISlots()
    {
        for (int i = 0; i < equipmentItemSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentItemSlots[i].slotType)
                {
                    equipmentItemSlots[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanupSlot();
        }

        for (int i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanupSlot();
        }

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(inventoryItems[i]);
        }

        for (int i = 0; i < stashItems.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stashItems[i]);
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData item)
    {
        if (item.itemType == itemType.Equipment && CanAddItem())
        {
            AddToInventory(item);
        }
        else if (item.itemType == itemType.Material)
        {
            AddToStash(item);
        }


        UpdateUISlots();
    }

    private void AddToStash(ItemData item)
    {
        // Check if the item is already in the stash
        if (stashDictionary.TryGetValue(item, out InventoryItem value))
        {
            // If it is, add a stack to the item
            value.AddStack();
        }
        else
        {
            // If it isn't, create a new inventory item and add it to the stash items list and dictionary
            InventoryItem newItem = new InventoryItem(item);
            stashItems.Add(newItem);
            stashDictionary.Add(item, newItem);
        }
    }

    private void AddToInventory(ItemData item)
    {
        // Check if the item is already in the inventory
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            // If it is, add a stack to the item
            value.AddStack();
        }
        else
        {
            // If it isn't, create a new inventory item and add it to the inventory items list and dictionary
            InventoryItem newItem = new InventoryItem(item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(item, newItem);
        }
    }

    public bool CanAddItem()
    {
        if(inventoryItems.Count >= inventoryItemSlots.Length)
        {
            return false;
        }

        return true;
    }

    public void RemoveItem(ItemData item)
    {
        // Check if the item is in the inventory
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            // If it is, check if the stack size is less than or equal to 1
            if (value.stackSize <= 1)
            {
                // If it is, remove the item from the inventory items list and dictionary
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(item);
            }
            else
            {
                // If it isn't, remove a stack from the item
                value.RemoveStack();
            }
        }

        if(stashDictionary.TryGetValue(item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stashItems.Remove(stashValue);
                stashDictionary.Remove(item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateUISlots();
    }

    public void EquipItem(ItemData _item)
    {
        // Cast the item to an ItemDataEquipment
        ItemDataEquipment newEquipment = _item as ItemDataEquipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemDataEquipment oldEquipment = null;

        // Check if the item is already equipped and unequip it
        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        // If there is an item equipped in the same slot, unequip it and add it back to the inventory
        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        // Add the new item to the equipment list and dictionary and add the modifiers
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        // Remove the item from the inventory
        RemoveItem(_item);

        // Update the UI slots
        UpdateUISlots();
    }

    public void UnequipItem(ItemDataEquipment itemToRemove)
    {
        // Check if the item is in the equipment dictionary and remove it and the modifiers
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    // This function is called when the player wants to craft an item
    public bool CanCraft(ItemDataEquipment itemToCraft, List<InventoryItem> requiredMaterials)
    {
        // Check if the item is already in the inventory
        foreach (var requiredItem in requiredMaterials)
        {
            if(stashDictionary.TryGetValue(requiredItem.itemData, out InventoryItem stashItem))
            {
                if(stashItem.stackSize < requiredItem.stackSize)
                {
                    // If the stash item stack size is less than the required item stack size, return false
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // if all the required items are in the stash, remove them from the stash
        foreach (var requiredItem in requiredMaterials)
        {
            for (int i = 0; i < requiredItem.stackSize; i++)
            {
                // Remove the required item from the stash
                RemoveItem(requiredItem.itemData);
            }
        }

        AddItem(itemToCraft);

        return true;
    }

    //returns the equipment list
    public List<InventoryItem> GetEquipmentList()
    {
        return equipment;
    }

    //returns the stash inventory list
    public List<InventoryItem> GetStashList()
    {
        return stashItems;
    }

    public ItemDataEquipment GetEquipment(EquipmentType _type)
    {
        ItemDataEquipment equippedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                equippedItem = item.Key;
            }
        }

        return equippedItem;
    }

    public void UseFlask()
    {
        ItemDataEquipment currentFlask = GetEquipment(EquipmentType.Flask);

        if(currentFlask == null)
        {
            return;
        }

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
        {
            Debug.Log("Flask is on cooldown");
        }
    }

    public bool CanUseArmour()
    {
        ItemDataEquipment currentArmour = GetEquipment(EquipmentType.Armor);

        if (Time.time > lastTimeUseArmour + ArmourCooldown)
        {
            ArmourCooldown = currentArmour.itemCooldown;
            lastTimeUseArmour = Time.time;
            return true;
        }

        return false;
    }

    public void LoadData(GameData _data)
    {
        //loop through the inventory dictionary and add the items to the inventory
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            // Check if the item is in the item database
            foreach (var item in itemDatabase)
            {
                // Check if the item ID matches the key in the dictionary
                if (item != null && item.itemID == pair.Key)
                {
                    // Create a new inventory item and set its stack size
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    // Add the item to the inventory items list and dictionary
                    loadedItems.Add(itemToLoad);
                }
            }
        }

        //loop through loadeditemid
        foreach (string loadedItemID in _data.equipmentID)
        {
            // Check if the item is in the item database
            foreach (var item in itemDatabase)
            {
                // Check if the item ID matches the key in the dictionary
                if (item != null && loadedItemID == item.itemID)
                {
                    //add the item to the loaded equipment list
                    loadedEquipment.Add(item as ItemDataEquipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        // Clear the inventory and equipment lists in the game data
        _data.inventory.Clear();
        _data.equipmentID.Clear();

        // Loop through the inventory dictionary and add the items to the game data
        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        // Loop through the stash dictionary and add the items to the game data
        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        // Loop through the equipment dictionary and add the items to the game data
        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentID.Add(pair.Key.itemID);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Fill up item data base")]
    private void FillupItemDatabase()
    {
        itemDatabase = new List<ItemData>(GetItemDatabase());
    }
    private List<ItemData> GetItemDatabase()
    {
        // Create a new list to store the item database
        List<ItemData> itemDatabase = new List<ItemData>();
        // Find all the item data assets in the specified folder
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/ScriptableObjects/Equipment" });

        // Loop through each asset name
        foreach (string SOName in assetNames)
        {
            // Get the asset path using the GUID
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            // Load the asset at the path and cast it to ItemData
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOPath);

            //add the item data to the item database list
            itemDatabase.Add(itemData);
        }

        // Return the item database list
        return itemDatabase;
    }
#endif
}
