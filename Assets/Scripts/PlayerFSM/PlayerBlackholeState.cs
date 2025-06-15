using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = 0.4f;
    private bool skillUsed;
    private float defaultGravity;

    public PlayerBlackholeState(PlayerStateMachine _stateMachine, PlayerController _playerController, string _animBoolName) : base(_stateMachine, _playerController, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = playerController.rb.gravityScale;
        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        playerController.rb.gravityScale = defaultGravity;
        playerController.entityFX.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15f);
        }

        if(stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.1f);

            if(!skillUsed)
            {
                if (playerController.skill.blackhole.CanUseSkill()) 
                { 
                    skillUsed = true; 
                }
            }
        }

        if(playerController.skill.blackhole.SkillCompleted())
        {
            stateMachine.ChangeState(playerController.AirState);
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
