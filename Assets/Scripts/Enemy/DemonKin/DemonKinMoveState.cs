using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonKinMoveState : DemonKinGroundedState
{
    public DemonKinMoveState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyDemonKin _enemy) : base(_stateMachine, _enemyBase, _animBoolName, _enemy)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        AudioManager.Instance.PlaySFX(10, enemy.transform);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(2 * enemy.facingDir, rb.velocity.y);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.IdleState);
        }
    }
}
