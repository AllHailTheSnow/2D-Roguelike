using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;
    public ItemEffect[] itemEffects;
    public float itemCooldown;
    [TextArea]
    public string itemEffectDescription;

    [Header("Main Stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Offensive Stats")]
    public int damage;
    public int critChance;
    public int critPower;

    [Header("Defensive Stats")]
    public int health;
    public int armour;
    public int evasion;
    public int magicResistance;

    [Header("Elemental Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;
    public int elementalChance;

    [Header("Craft Requirements")]
    public List<InventoryItem> craftingMaterials;

    // Add and remove modifiers to the player stats when the item is equipped or unequipped
    public void AddModifiers()
    {
        PlayerStats playerstats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        playerstats.strength.AddModifier(strength);
        playerstats.agility.AddModifier(agility);
        playerstats.intelligence.AddModifier(intelligence);
        playerstats.vitality.AddModifier(vitality);

        playerstats.damage.AddModifier(damage);
        playerstats.critChance.AddModifier(critChance);
        playerstats.critPower.AddModifier(critPower);

        playerstats.maxHP.AddModifier(health);
        playerstats.armour.AddModifier(armour);
        playerstats.evasion.AddModifier(evasion);
        playerstats.magicResistance.AddModifier(magicResistance);

        playerstats.fireDamage.AddModifier(fireDamage);
        playerstats.iceDamage.AddModifier(iceDamage);
        playerstats.lightningDamage.AddModifier(lightningDamage);
        playerstats.elementalChance.AddModifier(elementalChance);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerstats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        playerstats.strength.RemoveModifier(strength);
        playerstats.agility.RemoveModifier(agility);
        playerstats.intelligence.RemoveModifier(intelligence);
        playerstats.vitality.RemoveModifier(vitality);

        playerstats.damage.RemoveModifier(damage);
        playerstats.critChance.RemoveModifier(critChance);
        playerstats.critPower.RemoveModifier(critPower);

        playerstats.maxHP.RemoveModifier(health);
        playerstats.armour.RemoveModifier(armour);
        playerstats.evasion.RemoveModifier(evasion);
        playerstats.magicResistance.RemoveModifier(magicResistance);

        playerstats.fireDamage.RemoveModifier(fireDamage);
        playerstats.iceDamage.RemoveModifier(iceDamage);
        playerstats.lightningDamage.RemoveModifier(lightningDamage);
        playerstats.elementalChance.RemoveModifier(elementalChance);

    }

    // Execute the item effects when the item is used
    public void Effect(Transform enemyPos)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(enemyPos);
        }
    }

    private void AddItemDescription(int value, string name)
    {
        if (value != 0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if (value > 0)
            {
                sb.Append("+ " + value + " " + name);
            }
        }
    }

    public override string GetDescription()
    {
        sb.Length = 0; // Clear the StringBuilder

        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit Chance%");
        AddItemDescription(critPower, "Crit Power");

        AddItemDescription(health, "Health");
        AddItemDescription(armour, "Armour");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistance, "Magic Resist.");

        AddItemDescription(fireDamage, "Fire Dmg");
        AddItemDescription(iceDamage, "Ice Dmg");
        AddItemDescription(lightningDamage, "Lightning Dmg");
        AddItemDescription(elementalChance, "Elemental Chance");

        if(itemEffectDescription == null)
        {
            return null;
        }

        if(itemEffectDescription.Length > 0)
        {
            sb.AppendLine();
            sb.AppendLine();
            sb.Append(itemEffectDescription);
        }

        return sb.ToString();
    }
}
