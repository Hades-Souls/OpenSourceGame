using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class attack : MonoBehaviour
{
    public float attackdamage = 30;
    public float poiseDamgage = 20;
    public float attackredudce = 0f;
    Collider2D attackcollider;
    public bool ischarge;
    private bool ishit;
    private UIController uicontroller;
    private PlayerMovementMain Playermovement;
    private AttackChain PlayerAttack;

    public float knockbackForceX = 10f; // Adjust the knockback force for the x-axis as needed
    public float knockbackForceY = 10f; // Adjust the knockback force for the y-axis as needed

    private void Start()
    {
        attackcollider = GetComponent<Collider2D>();

    }
    private void Awake()
    {
        GameObject playerObject = GameObject.Find("Player");
        uicontroller = playerObject.GetComponent<UIController>();
        Playermovement = playerObject.GetComponent<PlayerMovementMain>();
        PlayerAttack = playerObject.transform.GetComponent<AttackChain>();
        ishit = false;
        if(ischarge)
        {
            knockbackForceX *= 1.5f;
            knockbackForceY *= 1.5f;
            attackdamage *= 2f;
            poiseDamgage *= 3f;
        }
    }
    private void Update()
    {
 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageAble damageAble = collision.GetComponent<DamageAble>();
        if(damageAble != null)
        {
            if(damageAble.poise <=0)
            {
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                    rb.velocity = Vector2.zero; // Reset the velocity to prevent accumulation of forces
                    Vector2 knockbackForce = new Vector2(knockbackDirection.x * knockbackForceX, knockbackDirection.y + knockbackForceY);
                    rb.AddForce(knockbackForce, ForceMode2D.Impulse);
                }

            }
            
            if(PlayerAttack.Isparry)
            {
                float dmgsdeal = 0f;
                float poisedmg = 0f;
                float attackReduce = 0f;
                attackReduce = PlayerAttack.parry; // parry value is already a percentage (e.g., 0.7 for 70%)
                float reductionAmount = attackdamage * attackReduce;
                dmgsdeal =  reductionAmount;
                poisedmg = reductionAmount;
                damageAble.Hit(dmgsdeal, poisedmg, reductionAmount, false);


            }
            else
            {
                damageAble.Hit(attackdamage, poiseDamgage, attackredudce, true);

            }
            


            if (collision.CompareTag("Enemy"))
            {
                if (damageAble != null)
                {
                    uicontroller.RecoverAfterAttack(ischarge, 1f);
                    
                }

            }
        }
 

    }


    public bool isHit()
    {
        return ishit;
    }
}
