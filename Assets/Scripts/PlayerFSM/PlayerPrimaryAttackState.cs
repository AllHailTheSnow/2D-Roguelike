using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int ComboCounter { get; private set; }
    public int SpecialCounter { get; private set; }

    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerPrimaryAttackState(PlayerStateMachine _stateMachine, PlayerController _playerController, string _animBoolName) : base(_stateMachine, _playerController, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.Instance.PlaySFX(6, null);

        xInput = 0;

        if(ComboCounter > 2|| Time.time >= lastTimeAttacked + comboWindow) { ComboCounter = 0; }

        playerController.anim.SetInteger("ComboCounter", ComboCounter);

        float attackDirection = playerController.facingDir;

        if(xInput != 0) { attackDirection = xInput; }

        playerController.SetVelocity(
            playerController.attackMovement[ComboCounter].x *
            attackDirection, playerController.attackMovement[ComboCounter].y);

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        playerController.StartCoroutine("BusyFor", 0.1f);

        ComboCounter++;
        //playerController.specialMeter += 5;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0) { playerController.SetVelocityZero(); }

        if (triggerCalled) { stateMachine.ChangeState(playerController.IdleState); }
    }

    public void ResetSpecialMeter()
    {
        playerController.specialMeter = 0;
    }
}
