using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlightSpellCastState : EnemyState
{
    private EnemyDeathBlight enemy;
    private int amountOfSpells;
    private float spellTimer;

    public DeathBlightSpellCastState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, EnemyDeathBlight _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        amountOfSpells = Random.Range(2, enemy.amountOfSpells + 1);
        spellTimer = .5f;
        AudioManager.Instance.PlaySFX(23, null);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastCastTime = Time.time;

        AudioManager.Instance.StopSFX(23);
    }

    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;

        if(CanCast())
        {
            enemy.CastSpell();
        }

        if(amountOfSpells <= 0)
        {
            stateMachine.ChangeState(enemy.TeleportState);
        }
    }

    private bool CanCast()
    {
        if(amountOfSpells > 0 && spellTimer < 0)
        {
            amountOfSpells--;
            spellTimer = enemy.spellCooldown;
            return true;
        }

        return false;
    }
}
