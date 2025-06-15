using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    private Enemy enemy => GetComponentInParent<Enemy>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            AudioManager.Instance.PlaySFX(enemy.missAttackSFX, enemy.transform);

            if (hit.GetComponent<PlayerController>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                AudioManager.Instance.PlaySFX(enemy.hitAttackSFX, enemy.transform);
                enemy.stats.DoDamage(target);
                PlayerManager.Instance.player.entityFX.ScreenShake(enemy.entityFX.screenShakeSwordCatch);
            }
        }
    }

    private void OpenCounterWindow()
    {
        enemy.OpenCounterAttackWindow();
    }

    private void CloseCounterWindow()
    {
        enemy.CloseCounterAttackWindow();
    }

    private void MovementSound()
    {
        AudioManager.Instance.PlaySFX(enemy.movementSFX, enemy.transform);
    }
}
