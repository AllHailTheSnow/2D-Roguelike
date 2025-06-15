using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine _stateMachine, PlayerController _playerController, string _animBoolName) : base(_stateMachine, _playerController, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        playerController.SetVelocityZero();
        playerController.rb.isKinematic = true;
        playerController.GroundCheck(0.15f);
    }

    public override void Exit()
    {
        base.Exit();
        playerController.rb.isKinematic = false;
    }

    public override void Update()
    {
        base.Update();

        if(xInput == playerController.facingDir && playerController.IsWallDetected()) { return; }

        if(xInput != 0 && !playerController.isBusy) { stateMachine.ChangeState(playerController.MoveState); }
    }
}
