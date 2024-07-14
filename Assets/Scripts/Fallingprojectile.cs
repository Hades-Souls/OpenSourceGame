using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallingprojectile : MonoBehaviour
{
    Rigidbody2D rb;
    public float fallingspeed = 1f;
    private Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(DelayedAction());

        
    }

    // Update is called once per frame
    void Update()
    {

            rb.velocity = new Vector2(rb.velocity.x,-fallingspeed);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
        animator.SetTrigger(AnimationStrings.hit);
        fallingspeed = 0f;
        Destroy(gameObject, 0.3f);

    }
    private IEnumerator DelayedAction()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(1f);
        fallingspeed =10f;


    }

}
