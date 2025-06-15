using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private Rigidbody2D rb;

    private void SetupVisuals()
    {
        if (itemData == null)
        {
            return;
        }

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object - " + itemData.name;
    }

    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }

    public void PickupItem()
    {
        if(!Inventory.Instance.CanAddItem() && itemData.itemType == itemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7);
            PlayerManager.Instance.player.entityFX.CreatePopupText("Inventory Full");
            return;
        }

        AudioManager.Instance.PlaySFX(12, null);
        Inventory.Instance.AddItem(itemData);
        PlayerManager.Instance.player.entityFX.CreatePopupText(itemData.name);
        Destroy(gameObject);
    }
}
