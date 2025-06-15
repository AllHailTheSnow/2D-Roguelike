using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDemonKin : Enemy
{
    public DemonKinIdleState IdleState { get; private set; }
    public DemonKinMoveState MoveState { get; private set; }
    public DemonKinStunnedState StunnedState { get; private set; }
    public DemonKinDeathState DeathState { get; private set; }
    public DemonKinBattleState BattleState { get; private set; }
    public DemonKinAttackState AttackState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        SetupDefaultFacingDir(1);

        IdleState = new DemonKinIdleState(stateMachine, this, "Idle", this);
        MoveState = new DemonKinMoveState(stateMachine, this, "Move", this);
        BattleState = new DemonKinBattleState(stateMachine, this, "Move", this);
        StunnedState = new DemonKinStunnedState(stateMachine, this, "Stunned", this);
        AttackState = new DemonKinAttackState(stateMachine, this, "Attack", this);
        DeathState = new DemonKinDeathState(stateMachine, this, "Death", this);
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
