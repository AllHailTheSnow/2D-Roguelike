using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWolf : Enemy
{
    public WolfIdleState IdleState { get; private set; }
    public WolfMoveState MoveState { get; private set; }
    public WolfBattleState BattleState { get; private set; }
    public WolfAttackState AttackState { get; private set; }
    public WolfDashAttackState DashAttackState { get; private set; }
    public WolfStunnedState StunnedState { get; private set; }
    public WolfDeathState DeathState { get; private set; }

    [Header("Dash Attack State Variables")]
    [SerializeField] public float DashSpeed;

    protected override void Awake()
    {
        base.Awake();
        SetupDefaultFacingDir(-1);

        IdleState = new WolfIdleState(stateMachine, this, "Idle", this);
        MoveState = new WolfMoveState(stateMachine, this, "Move", this);
        BattleState = new WolfBattleState(stateMachine, this, "Move", this);
        AttackState = new WolfAttackState(stateMachine, this, "Attack", this);
        DashAttackState = new WolfDashAttackState(stateMachine, this, "DashAttack", this);
        StunnedState = new WolfStunnedState(stateMachine, this, "Stunned", this);
        DeathState = new WolfDeathState(stateMachine, this, "Death", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(IdleState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(StunnedState);
            return true;
        }

        return false;
    }

    public override void Death()
    {
        base.Death();

        stateMachine.ChangeState(DeathState);
    }

}
