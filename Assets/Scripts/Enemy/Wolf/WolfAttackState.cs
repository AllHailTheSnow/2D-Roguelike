using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttackState : EnemyState
{
    protected EnemyWolf enemy;

    public WolfAttackState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyWolf _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
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
