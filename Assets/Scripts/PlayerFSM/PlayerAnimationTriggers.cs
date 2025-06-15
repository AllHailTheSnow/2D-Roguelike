using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private PlayerController playerController => GetComponentInParent<PlayerController>();

    private void AnimationTrigger()
    {
        playerController.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerController.attackCheck.position, playerController.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                EnemyStats target = hit.GetComponent<EnemyStats>();
                ItemDataEquipment weaponData = Inventory.Instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData != null)
                {
                    weaponData.Effect(target.transform);
                }

                if(target != null)
                {
                    if(!target.isInvincible && target.currentHP > 0)
                    {
                        AudioManager.Instance.PlaySFX(5, null);
                        playerController.specialMeter += 5;
                    }

                    playerController.stats.DoDamage(target);
                }
            }
        }
    }

    private void SpecialAttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerController.attackCheck.position, playerController.attackCheckRadius);

        int damage = playerController.stats.damage.GetValue() * 2;
        //int damage = 200;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats target = hit.GetComponent<EnemyStats>();
                ItemDataEquipment weaponData = Inventory.Instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData != null)
                {
                    weaponData.Effect(target.transform);
                }

                if (target != null)
                {
                    if (!target.isInvincible && target.currentHP > 0)
                    {
                        AudioManager.Instance.PlaySFX(5, null);
                        playerController.specialMeter += 5;
                    }

                    //playerController.stats.DoDamage(target);
                    target.TakeDamage(damage);
                }
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.Instance.sword.CreateSword();
    }
}
