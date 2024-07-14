using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullets : MonoBehaviour
{
    private float speed = 100f;
    public Rigidbody2D rb;
    private Animator animator;
    void Start()
    {


        animator = GetComponent<Animator>();    
        rb = GetComponent<Rigidbody2D>();


        if (rb != null) // Ensure the Rigidbody2D component is found
        {
            rb.velocity = transform.right * speed; // Moves the bullet to the right.
        }
        else
        {
            Debug.LogError("Rigidbody2D component not found on the GameObject.");
        }


       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("wall"))
        {
            animator.SetBool("Hit", true);
            Destroy(gameObject, 0.5f);

        } 
        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            Destroy(gameObject, 0.5f);
            if (enemyHealth != null)
            {
                animator.SetTrigger("Hit");
                enemyHealth.ReduceHealth(30f);
            }

        }
        else
        {
            Destroy(gameObject, 3f);
        }

    }


}
