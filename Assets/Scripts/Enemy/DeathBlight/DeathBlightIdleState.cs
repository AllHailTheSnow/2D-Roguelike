using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlightIdleState : EnemyState
{
    private EnemyDeathBlight enemy;
    private Transform player;

    public DeathBlightIdleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyDeathBlight _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
        player = PlayerManager.Instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(player.transform.position, enemy.transform.position) < 7)
        {
            enemy.bossFightBegun = true;
        }

        if (stateTimer < 0 && enemy.bossFightBegun)
        {
            stateMachine.ChangeState(enemy.BattleState);
        }
    }
}
