using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfStunnedState : EnemyState
{
    protected EnemyWolf enemy;

    public WolfStunnedState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyWolf _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.entityFX.InvokeRepeating("RedColourBlink", 0, 0.1f);

        stateTimer = enemy.stunDuration;

        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0) { stateMachine.ChangeState(enemy.IdleState); }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.entityFX.Invoke("CancelColourChange", 0);
    }
}
