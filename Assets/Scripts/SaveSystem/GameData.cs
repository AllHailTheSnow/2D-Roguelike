using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //Inventory data
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentID;

    //Skill tree data
    public SerializableDictionary<string, bool> skillTree;

    //Location data
    public SerializableDictionary<string, bool> checkPoints;
    public string closestCheckpointID;

    //Currency data
    public int currency;
    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    //Options data
    public SerializableDictionary<string, float> volumeSettings;

    public GameData()
    {
        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;

        this.currency = 0;
        inventory = new SerializableDictionary<string, int>();
        skillTree = new SerializableDictionary<string, bool>();
        equipmentID = new List<string>();

        closestCheckpointID = string.Empty;
        checkPoints = new SerializableDictionary<string, bool>();

        volumeSettings = new SerializableDictionary<string, float>();
    }
}
