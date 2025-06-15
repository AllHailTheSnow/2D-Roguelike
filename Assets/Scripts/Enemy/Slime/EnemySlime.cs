using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType
{
    big,
    medium,
    small
}

public class EnemySlime : Enemy
{
    [Header("Slime Properties")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int slimesToCreate;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minCreationVelocity;
    [SerializeField] private Vector2 maxCreationVelocity;

    #region States
    public SlimeIdleState IdleState { get; private set; }
    public SlimeMoveState MoveState { get; private set; }
    public SlimeBattleState BattleState { get; private set; }
    public SlimeAttackState AttackState { get; private set; }
    public SlimeStunnedState StunnedState { get; private set; }
    public SlimeDeathState DeathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        IdleState = new SlimeIdleState(stateMachine, this, "Idle", this);
        MoveState = new SlimeMoveState(stateMachine, this, "Move", this);
        BattleState = new SlimeBattleState(stateMachine, this, "Move", this);
        AttackState = new SlimeAttackState(stateMachine, this, "Attack", this);
        StunnedState = new SlimeStunnedState(stateMachine, this, "Stunned", this);
        DeathState = new SlimeDeathState(stateMachine, this, "Death", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
        if(base.CanBeStunned())
        {
            stateMachine.ChangeState(StunnedState);
            return true;
        }

        return false;
    }

    private void CreateSlimes(int _amountOfSlimes, GameObject _slimePrefab)
    {
        for (int i = 0; i < _amountOfSlimes; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);

            newSlime.GetComponent<EnemySlime>().SetupSlime(facingDir);
        }
    }

    public void SetupSlime(int _facingDir)
    {
        if(_facingDir != facingDir)
        {
            Flip();
        }

        float xVelocity = Random.Range(minCreationVelocity.x, maxCreationVelocity.x);
        float yVelocity = Random.Range(minCreationVelocity.y, maxCreationVelocity.y);

        isKnocked = true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * -facingDir, yVelocity);

        Invoke("CancelKnockback", 1.5f);
    }

    private void CancelKnockback()
    {
        isKnocked = false;
    }

    public override void Death()
    {
        base.Death();

        stateMachine.ChangeState(DeathState);

        if (slimeType == SlimeType.small)
        {
            return;
        }

        CreateSlimes(slimesToCreate, slimePrefab);
    }
}
