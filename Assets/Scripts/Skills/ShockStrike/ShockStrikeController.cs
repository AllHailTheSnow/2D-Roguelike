using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private int damage;

    private Animator animator;
    private bool triggered;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(!targetStats) { return; }

        if(triggered) {  return; }

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {
            animator.transform.localRotation = Quaternion.identity;
            animator.transform.localPosition = new Vector3(0, 0.55f);
            transform.localRotation = Quaternion.identity;

            triggered = true;
            animator.SetTrigger("Hit");
            Invoke(nameof(DamageDestroySelf), 0.2f);
        }
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    private void DamageDestroySelf()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, .5f);
    }
}
