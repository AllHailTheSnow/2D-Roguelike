using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>, ISaveManager
{
    public PlayerController player;
    public int currency;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        
    }

    public bool HaveEnoughMoney(int _price)
    {
        if(_price > currency)
        {
            return false;
        }

        currency = currency - _price;
        return true;
    }

    public int GetCurrency()
    {
        return currency;
    }

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }
}
