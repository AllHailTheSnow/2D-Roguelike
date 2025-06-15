using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private PlayerController player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colourFadeSpeed;

    private float cloneTimer;
    private float attackMultiplier;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.8f;
    private int facingDir = 1;

    private bool canDuplicateClone;
    private float chanceToDuplicate;

    //[SerializeField] private float closesetEnemyCheckRadius = 25;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private Transform closestEnemy;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if(cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime * colourFadeSpeed);

            if(sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetupClone(Transform newPos, float cloneDuration, bool canAttack, Vector3 offset, 
        Transform _clostestEnemy, bool _canDuplicateClone, float _chanceToDuplictae, PlayerController _player, float _attackMultiplier)
    {
        if(canAttack)
        {
            anim.SetInteger("attackNumber", Random.Range(1, 3));
        }

        transform.position = newPos.position + offset;

        player = _player;
        cloneTimer = cloneDuration;
        closestEnemy = _clostestEnemy;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplictae;
        attackMultiplier = _attackMultiplier;

        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //player.stats.DoDamage(hit.GetComponent<CharacterStats>());

                //knockback direction
                hit.GetComponent<Entity>().SetupKnockbackDirection(transform);

                //gets the enemy stats and player stats
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                //calls the player stats to do damage with clone and attack multiplier
                playerStats.CloneDamage(enemyStats, attackMultiplier);
                AudioManager.Instance.PlaySFX(5, null);


                //Allows the clone to use the weapon effect
                if (player.skill.clone.cloneAttackElementUnlocked)
                {
                    ItemDataEquipment weaponData = Inventory.Instance.GetEquipment(EquipmentType.Weapon);

                    if (weaponData != null)
                    {
                        weaponData.Effect(hit.transform);
                    }
                }

                //If clone can be duplicated checks chance and creates a clone
                if (canDuplicateClone)
                {
                    if(Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.Instance.clone.CreateClone(hit.transform, new Vector3(0.5f * facingDir, 0.7f));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if(transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
