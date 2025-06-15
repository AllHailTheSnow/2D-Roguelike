using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int amountOfItems;
    [SerializeField] private ItemData[] itemDropPool;
    private List<ItemData> possibleDrops = new();

    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        if(itemDropPool.Length == 0)
        {
            Debug.Log("No possible Drops");
            return;
        }

        foreach (ItemData item in itemDropPool)
        {
            if(item != null && Random.Range(0, 100) < item.dropChance)
            {
                possibleDrops.Add(item);
            }
        }

        for (int i = 0; i < amountOfItems; i++)
        {
            if(possibleDrops.Count > 0)
            {
                int randomIndex = Random.Range(0, possibleDrops.Count);
                ItemData itemToDrop = possibleDrops[randomIndex];

                DropItem(itemToDrop);
                possibleDrops.Remove(itemToDrop);
            }
        }

        //int randomDropChance = Random.Range(0, 100);
        //Debug.Log("Random Drop Chance: " + randomDropChance);

        //for (int i = 0; i < itemDropPool.Length; i++)
        //{
        //    if(randomDropChance <= itemDropPool[i].dropChance)
        //    {
        //        possibleDrops.Add(itemDropPool[i]);
        //    }
        //    else if (randomDropChance > itemDropPool[i].dropChance)
        //    {
        //        return;
        //    }
        //}

        //for (int i = 0; i < amountOfItems; i++)
        //{
        //    ItemData randomItem = possibleDrops[Random.Range(0, possibleDrops.Count - 1)];

        //    possibleDrops.Remove(randomItem);
        //    DropItem(randomItem);
        //}
    }

    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new(Random.Range(-5f, 5f), Random.Range(12f, 15f));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
