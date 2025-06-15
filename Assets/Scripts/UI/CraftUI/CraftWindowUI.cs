using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftWindowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemImage;

    [SerializeField] private Image[] materialImage;
    [SerializeField] private Button craftButton;

    public void SetupCraftWindow(ItemDataEquipment _item)
    {
        craftButton.onClick.RemoveAllListeners();

        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < _item.craftingMaterials.Count; i++)
        {
            materialImage[i].sprite = _item.craftingMaterials[i].itemData.icon;
            materialImage[i].color = Color.white;

            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().text = _item.craftingMaterials[i].stackSize.ToString();
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }

        itemImage.sprite = _item.icon;
        itemName.text = _item.itemName;
        itemDescription.text = _item.GetDescription();

        craftButton.onClick.AddListener(() =>
        {
            Inventory.Instance.CanCraft(_item, _item.craftingMaterials);
        });
    }


}
