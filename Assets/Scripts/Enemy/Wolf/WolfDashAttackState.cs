using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfDashAttackState : EnemyState
{
    protected EnemyWolf enemy;
    protected Transform player;

    public WolfDashAttackState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyWolf _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.player.transform;

        enemy.moveSpeed = enemy.DashSpeed;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.moveSpeed = enemy.defaultMoveSpeed;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);
        Vector2.MoveTowards(enemy.transform.position, player.position, enemy.moveSpeed * Time.deltaTime);

        if (triggerCalled) { stateMachine.ChangeState(enemy.BattleState); }
    }
}
