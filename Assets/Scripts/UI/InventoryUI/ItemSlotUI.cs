using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UIController uiController;
    public InventoryItem item;

    protected virtual void Start()
    {
        uiController = GetComponentInParent<UIController>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.itemData.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanupSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null) { return; }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.Instance.RemoveItem(item.itemData);
            return;
        }

        if(item.itemData.itemType == itemType.Equipment)
        {
            Inventory.Instance.EquipItem(item.itemData);
        }

        uiController.toolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null || item.itemData == null) { return; }

        uiController.toolTip.ShowToolTip(item.itemData as ItemDataEquipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null || item.itemData == null) { return; }

        uiController.toolTip.HideToolTip();
    }
}
