using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerStateMachine _stateMachine, PlayerController _playerController, string _animBoolName) : base(_stateMachine, _playerController, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        AudioManager.Instance.PlaySFX(7, null);
    }

    public override void Enter()
    {
        base.Enter();
        playerController.GroundCheck(1f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        playerController.SetVelocity(xInput * playerController.movementSpeed, rb.velocity.y);

        base.Update();

        if (xInput == 0 || playerController.IsWallDetected()) { stateMachine.ChangeState(playerController.IdleState); }
    }
}
