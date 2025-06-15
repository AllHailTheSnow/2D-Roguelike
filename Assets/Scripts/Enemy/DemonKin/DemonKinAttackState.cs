using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonKinAttackState : EnemyState
{
    protected EnemyDemonKin enemy;

    public DemonKinAttackState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyDemonKin _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocityZero();

        if (triggerCalled) { stateMachine.ChangeState(enemy.BattleState); }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastAttackTime = Time.time;
    }
}
