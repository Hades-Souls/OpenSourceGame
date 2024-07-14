using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;
    public float movespeed = 5f;
    public bool direction;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Make sure your GameObject has an Animator component.
        if(!direction)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (direction)
        {
            rb.velocity = new Vector2(movespeed, rb.velocity.y);

        }
        else
        {
            rb.velocity = new Vector2(-movespeed, rb.velocity.y);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
        animator.SetTrigger(AnimationStrings.hit);
        movespeed = 1f;
        Destroy(gameObject, 1f);
        
    }
}
