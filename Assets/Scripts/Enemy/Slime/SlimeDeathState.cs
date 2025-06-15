using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeathState : EnemyState
{
    private EnemySlime enemy;

    public SlimeDeathState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemySlime _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 0.5f;

    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            enemy.DeathFade();
        }
    }
}
