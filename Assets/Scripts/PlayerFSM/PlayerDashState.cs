using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerStateMachine _stateMachine, PlayerController _playerController, string _animBoolName) : base(_stateMachine, _playerController, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        playerController.skill.dash.CloneOnDash();

        stateTimer = playerController.dashDuration;
        
        playerController.stats.MakeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();

        playerController.skill.dash.CloneOnArrival();

        playerController.SetVelocity(0, rb.velocity.y);
        
        playerController.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if(!playerController.IsGroundDetected() && playerController.IsWallDetected())
        {
            stateMachine.ChangeState(playerController.WallSlide);
        }

        playerController.SetVelocity(playerController.dashSpeed * playerController.dashDir, 0);

        if (stateTimer < 0 ) { stateMachine.ChangeState(playerController.IdleState); }

        playerController.entityFX.CreateAfterImage();
    }
}
