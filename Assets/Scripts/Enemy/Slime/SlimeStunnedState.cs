using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunnedState : EnemyState
{
    private EnemySlime enemy;

    public SlimeStunnedState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemySlime _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
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

    public override void Exit()
    {
        base.Exit();

        enemy.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if(rb.velocity.y < .1f && enemy.IsGroundDetected())
        {
            enemy.entityFX.Invoke("CancelColourChange", 0);
            enemy.anim.SetTrigger("StunFold");
            enemy.stats.MakeInvincible(true);
        }

        if(stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }
}
