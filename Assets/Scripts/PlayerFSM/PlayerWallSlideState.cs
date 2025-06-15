using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(PlayerStateMachine _stateMachine, PlayerController _playerController, string _animBoolName) : base(_stateMachine, _playerController, _animBoolName)
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

        //if player is not touching the wall, change to air state
        if (!playerController.IsWallDetected()) { stateMachine.ChangeState(playerController.AirState); }

        //if player presses jump button, change to wall jump state
        if (Input.GetKeyDown(KeyCode.Space)) { stateMachine.ChangeState(playerController.WallJump);  return; }

        //if player is not pressing the wall, change to idle state
        if (xInput != 0 && playerController.facingDir != xInput) { stateMachine.ChangeState(playerController.IdleState); }

        //if yinput is less than 0
        if(yInput < 0)
        {
            //set the x velocity to 0 and the y velocity to the wall slide speed
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            //set the x velocity to 0 and the y velocity to the wall slide speed * 0.7
            rb.velocity = new Vector2(0, rb.velocity.y * 0.7f);
        }

        //if player is touching the ground, change to idle state
        if (playerController.IsGroundDetected()) { stateMachine.ChangeState(playerController.IdleState); }
    }
}
