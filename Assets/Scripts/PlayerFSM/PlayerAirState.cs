using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(PlayerStateMachine _stateMachine, PlayerController _playerController, string _animBoolName) : base(_stateMachine, _playerController, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (playerController.IsGroundDetected()) { stateMachine.ChangeState(playerController.IdleState); }

        if(playerController.IsWallDetected()) { stateMachine.ChangeState(playerController.WallSlide); }

        if (xInput != 0) { playerController.SetVelocity(playerController.movementSpeed * 0.8f * xInput, rb.velocity.y); }
    }
}
