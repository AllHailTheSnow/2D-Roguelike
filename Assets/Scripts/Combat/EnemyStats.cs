using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop itemDrop;
    public Stats currencyDropAmount;

    [Header("Level Details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float levelUpPercentage = 0.4f;

    protected override void Start()
    {
        currencyDropAmount.SetDefaultValue(100);
        ApplyLevelModifiers();

        base.Start();
        enemy = GetComponent<Enemy>();
        itemDrop = GetComponent<ItemDrop>();

    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHP);
        Modify(armour);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);

        Modify(currencyDropAmount);

    }

    private void Modify(Stats stat)
    {
        // Increase the stats of the enemy based on the level up percentage
        for (int i = 1; i < level; i++)
        {
            float modifier = stat.GetValue() * levelUpPercentage;

            stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Death()
    {
        base.Death();
        itemDrop.GenerateDrop();
        enemy.Death();
        PlayerManager.Instance.currency += currencyDropAmount.GetValue();
    }
}
