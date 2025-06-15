using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JurogumoSkillController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private int damage;
    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    private CharacterStats stats;
    private int facingDir = -1;

    private void Update()
    {
        rb.velocity = new Vector2(xVelocity, rb.velocity.y);

        if (facingDir == -1 && rb.velocity.x > 0)
        {
            facingDir = 1;
            sr.flipX = true;
        }
    }

    public void SetupSpell(float _speed, CharacterStats _stats)
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        xVelocity = _speed;
        stats = _stats;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CharacterStats>()?.isInvincible == true)
        {
            return;
        }

        if(collision.GetComponent<PlayerController>() != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            stats.DoDamage(collision.GetComponent<CharacterStats>());
            anim.SetTrigger("Hit");
            AudioManager.Instance.PlaySFX(29, transform);
            Destroy(gameObject, 0.2f);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
