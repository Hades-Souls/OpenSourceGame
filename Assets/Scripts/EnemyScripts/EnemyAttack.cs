using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackDamage = 30;
    Collider2D attackCollider;
    public bool StaggerAttack;
    private bool isHit;


    public float knockbackForceX = 10f; // Adjust the knockback force for the x-axis as needed
    public float knockbackForceY = 10f; // Adjust the knockback force for the y-axis as needed

    private void Start()
    {
        attackCollider = GetComponent<Collider2D>();
    }

    private void Awake()
    {
        GameObject playerObject = GameObject.Find("Player");

        isHit = false;

        if (StaggerAttack)
        {
            knockbackForceX *= 1.5f;
            knockbackForceY *= 1.5f;
            attackDamage *= 2f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerDamagable playerdamageAble = collision.GetComponent<PlayerDamagable>();
        if (playerdamageAble != null)
        {
            if (collision.CompareTag("Player"))
            {
                playerdamageAble.Hit(attackDamage, StaggerAttack);
                // Apply knockback
                Rigidbody2D targetRigidbody = collision.GetComponent<Rigidbody2D>();
                if (targetRigidbody != null)
                {
                    Vector2 knockbackDirection = collision.transform.position - transform.position;
                    knockbackDirection.Normalize();
                    Vector2 knockback = new Vector2(knockbackDirection.x * knockbackForceX, knockbackForceY);
                    targetRigidbody.AddForce(knockback, ForceMode2D.Impulse);
                }
            }
        }
    }

    public bool IsHit()
    {
        return isHit;
    }
}
