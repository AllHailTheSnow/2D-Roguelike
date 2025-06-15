using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected EnemySkeleton enemy;
    protected Transform playerPos;

    public SkeletonGroundedState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemySkeleton _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        playerPos = PlayerManager.Instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, playerPos.position) < 2)
        {
            stateMachine.ChangeState(enemy.BattleState);
        }
    }
}
