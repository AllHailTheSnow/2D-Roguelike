using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(PlayerStateMachine _stateMachine, PlayerController _playerController, string _animBoolName) : base(_stateMachine, _playerController, _animBoolName)
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

        if(!playerController.IsGroundDetected()) 
        { stateMachine.ChangeState(playerController.AirState); }

        if(Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && playerController.skill.sword.throwSwordUnlocked) 
        { stateMachine.ChangeState(playerController.AimSword); }

        if (Input.GetKeyDown(KeyCode.Space) && playerController.IsGroundDetected()) 
        { stateMachine.ChangeState(playerController.JumpState); }

        if(Input.GetKeyDown(KeyCode.Mouse0)) 
        { stateMachine.ChangeState(playerController.PrimaryAttack); }

        if (Input.GetKeyDown(KeyCode.Q) && playerController.skill.parry.parryUnlocked) 
        { stateMachine.ChangeState(playerController.CounterAttack); }

        if (Input.GetKeyDown(KeyCode.E) && playerController.specialMeter >= 100)
        { stateMachine.ChangeState(playerController.SpecialAttack); }

        if (Input.GetKeyDown(KeyCode.R) && playerController.skill.blackhole.blackholeUnlocked) 
        {
            if (playerController.skill.blackhole.cooldownTimer > 0)
            {
                playerController.entityFX.CreatePopupText("cooldown");
                return;
            }

            stateMachine.ChangeState(playerController.Blackhole); 
        }
    }

    private bool HasNoSword()
    {
        if(!playerController.sword) { return true; }

        playerController.sword.GetComponent<SwordSkillController>().ReturnSword();

        return false;
    }
}
