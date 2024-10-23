using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttac : MonoBehaviour
{
    public float attackDamage = 30;
    public float poiseDamage = 20;
    public float attackReduce = 0f;
    Collider2D attackCollider;
    public bool isCharge;
    private bool isHit;
    private UIController uiController;
    private PlayerMovementMain playerMovement;
    private AttackChain playerAttack;

    public float knockbackForceX = 10f; // Adjust the knockback force for the x-axis as needed
    public float knockbackForceY = 10f; // Adjust the knockback force for the y-axis as needed

    private void Start()
    {
        attackCollider = GetComponent<Collider2D>();
    }

    private void Awake()
    {
        GameObject playerObject = GameObject.Find("Player");
        uiController = playerObject.GetComponent<UIController>();
        playerMovement = playerObject.GetComponent<PlayerMovementMain>();
        playerAttack = playerObject.transform.GetComponent<AttackChain>();
        isHit = false;

        if (isCharge)
        {
            knockbackForceX *= 1.5f;
            knockbackForceY *= 1.5f;
            attackDamage *= 2f;
            poiseDamage *= 3f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageAble damageAble = collision.GetComponent<DamageAble>();
        if (damageAble != null)
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log("hit");
                damageAble.Hit(attackDamage);

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
