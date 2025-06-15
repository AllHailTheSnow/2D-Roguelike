using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttackState : PlayerState
{
    public PlayerSpecialAttackState(PlayerStateMachine _stateMachine, PlayerController _playerController, string _animBoolName) : base(_stateMachine, _playerController, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.Instance.PlaySFX(6, null);
        playerController.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
        playerController.StartCoroutine("BusyFor", 0.15f);
        playerController.PrimaryAttack.ResetSpecialMeter();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled) { stateMachine.ChangeState(playerController.IdleState); }
    }
}
