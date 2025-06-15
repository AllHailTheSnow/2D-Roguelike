using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JurogumoDeathState : EnemyState
{
    private EnemyJurogumo enemy;
    public JurogumoDeathState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyJurogumo _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.Instance.PlaySFX(1, enemy.transform);
        AudioManager.Instance.PlayBGM(0);

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
