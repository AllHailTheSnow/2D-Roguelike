using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    #region States
    public SkeletonIdleState IdleState { get; private set; }
    public SkeletonMoveState MoveState { get; private set; }
    public SkeletonBattleState BattleState { get; private set; }
    public SkeletonAttackState AttackState { get; private set; }
    public SkeletonStunnedState StunnedState { get; private set; }
    public SkeletonDeathState DeathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        SetupDefaultFacingDir(-1);

        IdleState = new SkeletonIdleState(stateMachine, this, "Idle", this);
        MoveState = new SkeletonMoveState(stateMachine, this, "Move", this);
        BattleState = new SkeletonBattleState(stateMachine, this, "Move", this);
        AttackState = new SkeletonAttackState(stateMachine, this, "Attack", this);
        StunnedState = new SkeletonStunnedState(stateMachine, this, "Stunned", this);
        DeathState = new SkeletonDeathState(stateMachine, this, "Death", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();

        //if(Input.GetKeyDown(KeyCode.L))
        //{
        //    stateMachine.ChangeState(StunnedState);
        //}
    }

    public override bool CanBeStunned()
    {
        if(base.CanBeStunned())
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
