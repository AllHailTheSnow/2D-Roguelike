using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(PlayerStateMachine _stateMachine, PlayerController _playerController, string _animBoolName) : base(_stateMachine, _playerController, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = .5f;
        playerController.SetVelocity(5 * -playerController.facingDir, playerController.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0) { stateMachine.ChangeState(playerController.AirState); }

        if (playerController.IsGroundDetected()) { stateMachine.ChangeState(playerController.IdleState); }
    }
}
