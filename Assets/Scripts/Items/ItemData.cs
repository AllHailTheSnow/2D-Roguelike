using System.Text;
using UnityEditor;
using UnityEngine;
public enum itemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public itemType itemType;
    public string itemName;
    public Sprite icon;
    public string itemID;

    [Range(1, 100)]
    public int dropChance;

    protected StringBuilder sb = new();

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual string GetDescription()
    {
        return "";
    }
}
