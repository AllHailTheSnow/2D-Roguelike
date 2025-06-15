using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJurogumo : Enemy
{
    [Header("Jurogumo Spell Variables")]
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private Transform spellCastPoint;
    [SerializeField] private float spellVelocity;
    public int amountOfSpells;
    public float spellCooldown;
    public float lastCastTime;
    [SerializeField] private float spellStateCooldown;

    public JurogumoIdleState IdleState { get; private set; }
    public JurogumoMoveState MoveState { get; private set; }
    public JurogumoAttackState AttackState { get; private set; }
    public JurogumoSpellCastState SpellCastState { get; private set; }
    public JurogumoBattleState BattleState { get; private set; }
    public JurogumoDeathState DeathState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        SetupDefaultFacingDir(-1);

        IdleState = new JurogumoIdleState(stateMachine, this, "Idle", this);
        MoveState = new JurogumoMoveState(stateMachine, this, "Move", this);
        AttackState = new JurogumoAttackState(stateMachine, this, "Attack", this);
        SpellCastState = new JurogumoSpellCastState(stateMachine, this, "SpellCast", this);
        BattleState = new JurogumoBattleState(stateMachine, this, "Move", this);
        DeathState = new JurogumoDeathState(stateMachine, this, "Death", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(IdleState);
    }

    public override void Death()
    {
        base.Death();

        stateMachine.ChangeState(DeathState);
    }

    public bool CanCastSpell()
    {
        if (Time.time >= lastCastTime + spellStateCooldown)
        {
            return true;
        }

        return false;
    }

    public void CastSpell()
    {
        GameObject newSpell = Instantiate(spellPrefab, spellCastPoint.position, Quaternion.identity);
        newSpell.GetComponent<JurogumoSkillController>().SetupSpell(spellVelocity * facingDir, stats);
    }
}
