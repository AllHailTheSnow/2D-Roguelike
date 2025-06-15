using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBattleState : EnemyState
{
    protected EnemyWolf enemy;
    protected Transform player;
    private int moveDir;

    public WolfBattleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyWolf _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.AttackState);
                }
            }
        }
        else
        {
            if (Vector2.Distance(player.transform.position, enemy.transform.position) > 4)
            {
                stateMachine.ChangeState(enemy.DashAttackState);
            }

            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
            {
                stateMachine.ChangeState(enemy.IdleState);
            }
        }

        if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance - .1f)
        {
            return;
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastAttackTime + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastAttackTime = Time.time;
            return true;
        }

        return false;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        AudioManager.Instance.PlaySFX(10, enemy.transform);
    }
}
