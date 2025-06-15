using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JurogumoSpellCastState : EnemyState
{
    private EnemyJurogumo enemy;
    private int amountOfSpells;
    private float spellTimer;
    public JurogumoSpellCastState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyJurogumo _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        amountOfSpells = Random.Range(1, enemy.amountOfSpells + 1);
        spellTimer = .5f;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastCastTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;

        if (CanCast())
        {
            enemy.CastSpell();
        }

        if (amountOfSpells <= 0)
        {
            stateMachine.ChangeState(enemy.BattleState);
        }
    }

    private bool CanCast()
    {
        if (amountOfSpells > 0 && spellTimer < 0)
        {
            amountOfSpells--;
            spellTimer = enemy.spellCooldown;
            return true;
        }

        return false;
    }
}
