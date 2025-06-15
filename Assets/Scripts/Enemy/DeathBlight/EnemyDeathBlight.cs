using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathBlight : Enemy
{
    public bool bossFightBegun;

    [Header("Spell Variables")]
    [SerializeField] private GameObject spellPrefab;
    public int amountOfSpells;
    public float spellCooldown;
    public float lastCastTime;
    [SerializeField] private float spellStateCooldown;
    [SerializeField] private Vector2 spellOffset;

    [Header("Teleport Variables")]
    [SerializeField] private BoxCollider2D teleportArea;
    [SerializeField] private Vector2 surroundingsCheck;
    public float chanceToTeleport;
    public float defaultChanceToTeleport = 25;

    #region States
    public DeathBlightAttackState AttackState { get; private set; }
    public DeathBlightSpellCastState SpellCastState { get; private set; }
    public DeathBlightBattleState BattleState { get; private set; }
    public DeathBlightDeathState DeathState { get; private set; }
    public DeathBlightIdleState IdleState { get; private set; }
    public DeathBlightTeleportState TeleportState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        IdleState = new DeathBlightIdleState(stateMachine, this, "Idle", this);
        BattleState = new DeathBlightBattleState(stateMachine, this, "Move", this);
        AttackState = new DeathBlightAttackState(stateMachine, this, "Attack", this);
        SpellCastState = new DeathBlightSpellCastState(stateMachine, this, "SpellCast", this);
        TeleportState = new DeathBlightTeleportState(stateMachine, this, "Teleport", this);
        DeathState = new DeathBlightDeathState(stateMachine, this, "Death", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Death()
    {
        base.Death();
        stateMachine.ChangeState(DeathState);
    }

    private RaycastHit2D GroundBelow()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    }

    private bool SomethingIsAround()
    {
        return Physics2D.BoxCast(transform.position, surroundingsCheck, 0, Vector2.zero, 0, whatIsGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingsCheck);
    }

    public void FindPosition()
    {
        float x = Random.Range(teleportArea.bounds.min.x + 3, teleportArea.bounds.max.x - 3);
        float y = Random.Range(teleportArea.bounds.min.y + 3, teleportArea.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (hitbox.size.y / 2));

        if (!GroundBelow() || SomethingIsAround())
        {
            FindPosition();
        }
    }

    public bool CanTeleport()
    {
        if(Random.Range(0, 100) <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }

        return false;
    }

    public bool CanCastSpell()
    {
        if(Time.time >= lastCastTime + spellStateCooldown)
        {
            return true;
        }

        return false;
    }

    public void CastSpell()
    {
        PlayerController player = PlayerManager.Instance.player;

        float xOffset = 0;

        if (player.rb.velocity.x != 0)
        {
            xOffset = player.facingDir * spellOffset.x;
        }

        Vector3 spellPos = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + spellOffset.y);

        GameObject newSpell = Instantiate(spellPrefab, spellPos, Quaternion.identity);
        newSpell.GetComponent<DeathBlightSkillController>().SetupSpell(stats);
    }
}
