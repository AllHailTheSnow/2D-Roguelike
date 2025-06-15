using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfDeathState : EnemyState
{
    protected EnemyWolf enemy;

    public WolfDeathState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyWolf _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.Instance.PlaySFX(24, enemy.transform);

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
