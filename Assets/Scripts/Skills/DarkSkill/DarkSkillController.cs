using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkSkillController : MonoBehaviour
{
    private PlayerController player;
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D attackCheck => GetComponent<CircleCollider2D>();

    private float darkTimer;

    // Dark Skill Moving
    private float moveSpeed;
    private bool moveToEnemy;
    private Transform clostestEnemy;

    // Dark Skill Explosion
    [SerializeField] private float growSpeed;
    private bool canExplode;
    private bool canGrow;
    [SerializeField] private LayerMask whatIsEnemy;

    private void Update()
    {
        darkTimer -= Time.deltaTime;

        if (darkTimer < 0)
        {
            FinishDark();
        }

        if(canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(2, 2), growSpeed * Time.deltaTime);
        }

        if(moveToEnemy)
        {
            if(clostestEnemy == null)
            {
                return;
            }

            transform.position = Vector2.MoveTowards(transform.position, clostestEnemy.position, moveSpeed * Time.deltaTime);
            
            if(Vector2.Distance(transform.position, clostestEnemy.position) < 1f)
            {
                moveToEnemy = false;
                FinishDark();
            }
        }
    }

    public void SetupDarkOrb(float _darkDuration, bool _canExplode, bool _moveToEnemy, float _moveSpeed, Transform _closestEnemy, PlayerController _player)
    {
        darkTimer = _darkDuration;
        canExplode = _canExplode;
        moveToEnemy = _moveToEnemy;
        moveSpeed = _moveSpeed;
        clostestEnemy = _closestEnemy;
        player = _player;
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackCheck.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDirection(transform);
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

                ItemDataEquipment equippedAccessory = Inventory.Instance.GetEquipment(EquipmentType.Accessory);

                if(equippedAccessory != null)
                {
                    equippedAccessory.Effect(hit.transform);
                }
            }
        }
    }

    public void FinishDark()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.Instance.blackhole.GetBlackholeRadius();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if(colliders.Length > 0)
        {
            clostestEnemy = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
