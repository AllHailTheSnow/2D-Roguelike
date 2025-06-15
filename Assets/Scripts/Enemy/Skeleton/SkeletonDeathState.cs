using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathState : EnemyState
{
    private EnemySkeleton enemy; 

    public SkeletonDeathState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemySkeleton _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.Instance.PlaySFX(14, enemy.transform);

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
