using System.Collections;
using UnityEngine;

public class PlayerController : Entity
{
    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration;
    public int specialMeterMax = 100;
    public int specialMeter;
    public bool isBusy { get; private set; }

    [Header("Movement Details")]
    public float movementSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact;
    public float defaultMoveSpeed;
    public float defaultJumpForce;

    [Header("Dash Details")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;
    public float dashDir { get; private set; }

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }
    public PlayerFX entityFX { get; private set; }

    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallJumpState WallJump { get; private set; }
    public PlayerWallSlideState WallSlide { get; private set; }
    public PlayerPrimaryAttackState PrimaryAttack { get; private set; }
    public PlayerCounterAttackState CounterAttack { get; private set; }
    public PlayerAimSwordState AimSword { get; private set; }
    public PlayerCatchSwordState CatchSword { get; private set; }
    public PlayerBlackholeState Blackhole { get; private set; }
    public PlayerDeathState DeathState { get; private set; }
    public PlayerSpecialAttackState SpecialAttack { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        //Movement states
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(StateMachine, this, "Idle");
        MoveState = new PlayerMoveState(StateMachine, this, "Move");
        JumpState = new PlayerJumpState(StateMachine, this, "Jump");
        AirState = new PlayerAirState(StateMachine, this, "Jump");
        DashState = new PlayerDashState(StateMachine, this, "Dash");
        WallSlide = new PlayerWallSlideState(StateMachine, this, "WallSlide");
        WallJump = new PlayerWallJumpState(StateMachine, this, "WallJump");

        //Combat states
        PrimaryAttack = new PlayerPrimaryAttackState(StateMachine, this, "Attack");
        CounterAttack = new PlayerCounterAttackState(StateMachine, this, "CounterAttack");
        SpecialAttack = new PlayerSpecialAttackState(StateMachine, this, "SpecialAttack");

        //Sword states
        AimSword = new PlayerAimSwordState(StateMachine, this, "AimSword");
        CatchSword = new PlayerCatchSwordState(StateMachine, this, "CatchSword");
        Blackhole = new PlayerBlackholeState(StateMachine, this, "Jump");

        //Death state
        DeathState = new PlayerDeathState(StateMachine, this, "Death");
    }

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.Instance;
        StateMachine.Initialize(IdleState);
        entityFX = GetComponent<PlayerFX>();

        defaultMoveSpeed = movementSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        base.Update();

        StateMachine.CurrentState.Update();

        CheckForDashInput();

        if(Input.GetKeyDown(KeyCode.F) && skill.dark.darkUnlocked)
        {
            skill.dark.CanUseSkill();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.Instance.UseFlask();
        }
    }

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if(IsWallDetected())
        {
            return;
        }
        if(skill.dash.dashUnlocked == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.dash.CanUseSkill())
        {

            dashDir = Input.GetAxisRaw("Horizontal");

            if(dashDir == 0)
            {
                dashDir = facingDir;
            }

            StateMachine.ChangeState(DashState);
        }
    }

    public void AssignNewSword(GameObject newSword)
    {
        sword = newSword;
    }

    public void CatchTheSword()
    {
        StateMachine.ChangeState(CatchSword);
        AudioManager.Instance.PlaySFX(18, null);
        Destroy(sword);
    }

    public override void SlowEntityBy(float _slowPercent, float _slowDuration)
    {
        movementSpeed = movementSpeed * (1 - _slowPercent);
        jumpForce = jumpForce * (1 - _slowPercent);
        dashSpeed = dashSpeed * (1 - _slowPercent);
        anim.speed = anim.speed * (1 - _slowPercent);

        Invoke(nameof(ReturnDefaultSpeed), _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        movementSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }

    public override void Death()
    {
        base.Death();

        StateMachine.ChangeState(DeathState);
    }

    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(seconds);

        isBusy = false;
    }
}
