using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private PlayerController player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<PlayerController>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        if(_damage > GetMaxHealth() * .3f)
        {
            int randomSound = Random.Range(15, 17);
            AudioManager.Instance.PlaySFX(randomSound, null);
            player.SetupKnockbackPower(new Vector2(20, 5));
            player.entityFX.ScreenShake(player.entityFX.screenShakeHighDamage);
        }

        //AudioManager.Instance.PlaySFX(16, null);

        ItemDataEquipment currentArmour = Inventory.Instance.GetEquipment(EquipmentType.Armor);

        if(currentArmour != null)
        {
            currentArmour.Effect(player.transform);
        }
    }

    public void CloneDamage(CharacterStats targetStats, float attackMultiplier)
    {
        //Check if the target can avoid the attack
        if (CheckTargetCanAvoid(targetStats))
        {
            return;
        }
        
        //Get the total damage from strength and damage
        int totalDamage = damage.GetValue() + strength.GetValue();

        if(attackMultiplier > 0)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * attackMultiplier);
        }

        //Check if the attack can crit
        if (CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }

        totalDamage = CheckTargetArmour(targetStats, totalDamage);
        targetStats.TakeDamage(totalDamage);
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateCloneDodge();
    }

    protected override void Death()
    {
        base.Death();

        player.Death();

        GameManager.Instance.lostCurrencyAmount = PlayerManager.Instance.currency;
        PlayerManager.Instance.currency = 0;

        GetComponent<PlayerItemDrop>().GenerateDrop();
    }
}
