using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;

    public PlayerCounterAttackState(PlayerStateMachine _stateMachine, PlayerController _playerController, string _animBoolName) : base(_stateMachine, _playerController, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;
        stateTimer = playerController.counterAttackDuration;
        playerController.anim.SetBool("SuccessfulCounter", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        playerController.SetVelocityZero();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerController.attackCheck.position, playerController.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;

                    playerController.anim.SetBool("SuccessfulCounter", true);

                    playerController.skill.parry.UseSkill(); // Drains health on parry

                    if(canCreateClone)
                    {
                        canCreateClone = false;
                        playerController.skill.parry.MakeCloneOnParry(hit.transform);
                    }
                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(playerController.IdleState);
        }
    }
}
