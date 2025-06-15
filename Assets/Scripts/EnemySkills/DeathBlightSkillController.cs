using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlightSkillController : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;

    private CharacterStats myStats;

    public void SetupSpell(CharacterStats _stats)
    {
        myStats = _stats;
    }

    private void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);

        foreach (var hit in colliders)
        {
            AudioManager.Instance.PlaySFX(22, hit.transform);
            if (hit.GetComponent<PlayerController>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDirection(transform);
                AudioManager.Instance.PlaySFX(22, hit.transform);
                PlayerManager.Instance.player.entityFX.ScreenShake(PlayerManager.Instance.player.entityFX.screenShakeHighDamage);
                myStats.DoDamage(hit.GetComponent<PlayerStats>());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position, boxSize);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
