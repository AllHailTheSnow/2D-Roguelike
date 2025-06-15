using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JurogumoGroundedState : EnemyState
{
    protected EnemyJurogumo enemy;
    protected Transform player;

    public JurogumoGroundedState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyJurogumo _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.Instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
        {
            stateMachine.ChangeState(enemy.BattleState);
        }
    }
}
