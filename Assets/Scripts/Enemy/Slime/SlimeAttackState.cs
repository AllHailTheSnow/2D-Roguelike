using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttackState : EnemyState
{
    protected EnemySlime enemy;
    public SlimeAttackState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemySlime _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastAttackTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocityZero();

        if(triggerCalled)
        {
            stateMachine.ChangeState(enemy.BattleState);
        }
    }
}
