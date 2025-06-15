using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    //Components
    protected PlayerStateMachine stateMachine;
    protected PlayerController playerController;

    protected Rigidbody2D rb;

    //Movement Variables
    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(PlayerStateMachine _stateMachine, PlayerController _playerController, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.playerController = _playerController;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
       playerController.anim.SetBool(animBoolName, true);
        rb = playerController.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        playerController.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        playerController.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
