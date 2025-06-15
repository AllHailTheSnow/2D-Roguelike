using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlightTeleportState : EnemyState
{
    private EnemyDeathBlight enemy;

    public DeathBlightTeleportState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyDeathBlight _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.stats.MakeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            if(enemy.CanCastSpell())
            {
                stateMachine.ChangeState(enemy.SpellCastState);
            }
            else
            {
                stateMachine.ChangeState(enemy.BattleState);
            }
        }
    }
}
