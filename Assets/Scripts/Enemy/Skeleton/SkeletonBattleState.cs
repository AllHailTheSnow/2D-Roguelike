using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform playerPos;
    private EnemySkeleton enemy;
    private int moveDir;

    public SkeletonBattleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemySkeleton _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        playerPos = PlayerManager.Instance.player.transform;

        if(playerPos.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }

    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if(CanAttack())
                {
                    stateMachine.ChangeState(enemy.AttackState);
                }
            }
        }
        else
        {
            if(stateTimer < 0 || Vector2.Distance(playerPos.transform.position, enemy.transform.position) > 7)
            {
                stateMachine.ChangeState(enemy.IdleState);
            }
        }

        if(playerPos.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (playerPos.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }

        //if(Vector2.Distance(enemy.transform.position, playerPos.position) < 2)
        //{
        //    enemy.SetVelocityZero();
        //}

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
        if(Time.time >= enemy.lastAttackTime + enemy.attackCooldown)
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
