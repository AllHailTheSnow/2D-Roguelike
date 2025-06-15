using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D hitbox { get; private set; }
    #endregion

    [Header("Knockback Properties")]
    [SerializeField] protected Vector2 knockbackPower = new Vector2(7, 12);
    [SerializeField] protected Vector2 knockbackOffset = new Vector2(0.5f, 2f);
    [SerializeField] protected float knockbackDuration = 0.07f;
    protected bool isKnocked;

    [Header("collision Properties")]
    public Transform attackCheck;
    public float attackCheckRadius = 1.2f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 1;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = .8f;
    [SerializeField] protected LayerMask whatIsGround;

    public int knockbackDir { get; private set; }
    public int facingDir { get; private set; } = 1;
    protected bool isFacingRight = true;

    public System.Action OnFlipped;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        hitbox = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }

    public void GroundCheck(float _groundCheckDistance)
    {
        groundCheckDistance = _groundCheckDistance;
    }

    public virtual void DamageImpact()
    {
        //_ = StartCoroutine(nameof(HitKnockbackRoutine));
        StartCoroutine(HitKnockbackRoutine());
    }

    public virtual void SlowEntityBy(float _slowPercent, float _slowDuration)
    {
        
    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void Death()
    {
        rb.gravityScale = 100;
    }

    #region Velocity
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if(isKnocked) { return; }

        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public void SetVelocityZero()
    {
        if(isKnocked) { return; }

        rb.velocity = Vector2.zero;
    }
    #endregion

    #region Collision
    public virtual bool IsGroundDetected()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    public virtual bool IsWallDetected()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
        if(OnFlipped != null)
        {
            OnFlipped();
        }
    }

    public virtual void FlipController(float x)
    {
        if (x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    public virtual void SetupDefaultFacingDir(int _direction)
    {
        facingDir = _direction;

        if(facingDir == -1)
        {
            isFacingRight = false;
        }
    }
    #endregion

    public void DeathFade()
    {
        StartCoroutine(DeathFadeRoutine());
    }

    public void SetupKnockbackDirection(Transform _damageDirection)
    {
        if(_damageDirection.position.x > transform.position.x)
        {
            knockbackDir = -1;
        }
        else if (_damageDirection.position.x < transform.position.x)
        {
            knockbackDir = 1;
        }
    }

    public void SetupKnockbackPower(Vector2 _knockbackPower)
    {
        knockbackPower = _knockbackPower;
    }

    protected virtual void SetupZeroKnockbackPower()
    {

    }

    protected virtual IEnumerator HitKnockbackRoutine()
    {
        isKnocked = true;

        //rb.velocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);

        float xOffset = Random.Range(knockbackOffset.x, knockbackOffset.y);

        if (knockbackPower.x > 0 || knockbackPower.y > 0)
        {
            rb.velocity = new Vector2((knockbackPower.x + xOffset) * knockbackDir, knockbackPower.y);
        }

        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
        SetupZeroKnockbackPower();
    }

    private IEnumerator DeathFadeRoutine()
    {
        float duration = 4f;
        float startAlpha = spriteRenderer.color.a;
        float endAlpha = 0;
        float elapsedTime = 0;

        while(elapsedTime < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            Color newColour = spriteRenderer.color;
            newColour.a = newAlpha;
            spriteRenderer.color = newColour;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Color finalColour = spriteRenderer.color;
        finalColour.a = endAlpha;
        spriteRenderer.color = finalColour;

        Destroy(gameObject);
    }
}
