using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JurogumoAttackState : EnemyState
{
    protected EnemyJurogumo enemy;
    public JurogumoAttackState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyJurogumo _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocityZero();

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

    public override void Exit()
    {
        base.Exit();

        enemy.lastAttackTime = Time.time;
    }
}
