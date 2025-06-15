using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlightAttackState : EnemyState
{
    private EnemyDeathBlight enemy;

    public DeathBlightAttackState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyDeathBlight _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.chanceToTeleport += 5;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocityZero();

        if (triggerCalled) 
        { 
            if(enemy.CanTeleport())
            {
                stateMachine.ChangeState(enemy.TeleportState);
            }
            else
            {
                stateMachine.ChangeState(enemy.BattleState); 
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastAttackTime = Time.time;
    }
}
