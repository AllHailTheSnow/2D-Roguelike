using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(PlayerStateMachine _stateMachine, PlayerController _playerController, string _animBoolName) : base(_stateMachine, _playerController, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        playerController.SetVelocityZero();
        playerController.skill.sword.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        playerController.StartCoroutine("BusyFor", .2f);
    }

    public override void Update()
    {
        base.Update();

        if(Input.GetButtonUp("Fire2")) { stateMachine.ChangeState(playerController.IdleState); }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(playerController.transform.position.x > mousePos.x && playerController.facingDir == 1)
        {
            playerController.Flip();
        }
        else if (playerController.transform.position.x < mousePos.x && playerController.facingDir == -1)
        {
            playerController.Flip();
        }
    }
}
