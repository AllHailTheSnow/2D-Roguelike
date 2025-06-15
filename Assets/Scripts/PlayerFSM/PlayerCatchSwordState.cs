using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    public PlayerCatchSwordState(PlayerStateMachine _stateMachine, PlayerController _playerController, string _animBoolName) : base(_stateMachine, _playerController, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = playerController.sword.transform;

        playerController.entityFX.DustFX();
        playerController.entityFX.ScreenShake(playerController.entityFX.screenShakeSwordCatch);


        if (playerController.transform.position.x > sword.position.x && playerController.facingDir == 1)
        {
            playerController.Flip();
        }
        else if (playerController.transform.position.x < sword.position.x && playerController.facingDir == -1)
        {
            playerController.Flip();
        }

        rb.velocity = new Vector2(playerController.swordReturnImpact * -playerController.facingDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        playerController.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled) {  stateMachine.ChangeState(playerController.IdleState); }
    }
}
