using UnityEngine.EventSystems;

public class CraftSlotUI : ItemSlotUI
{
    protected override void Start()
    {
        base.Start();
    }

   public void SetupCraftSlot(ItemDataEquipment _item)
    {
        if(_item == null)
        {
            return;
        }

        item.itemData = _item;

        itemImage.sprite = _item.icon;
        itemText.text = _item.itemName;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        uiController.craftWindowUI.SetupCraftWindow(item.itemData as ItemDataEquipment);
    }
}
