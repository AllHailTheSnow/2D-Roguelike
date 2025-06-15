using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIdleState : SlimeGroundedState
{
    public SlimeIdleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemySlime _enemy) : base(_stateMachine, _enemyBase, _animBoolName, _enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }
}
