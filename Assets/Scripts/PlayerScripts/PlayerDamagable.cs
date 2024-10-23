using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagable : MonoBehaviour
{
    private Animator animator;
    public float _maxHealth = 100f;
    [SerializeField]
    private float _health;
    private bool isAlive = true;
    private bool invincible = false;
    private CapsuleCollider2D boxCollider;
    public float poise = 100f;
    public float Maxpoise = 100f;
    public float counterDuration = 5.0f; // Duration for the counter in seconds

    private float parryhealth;
    public float totalAttackReduce; // To keep track of the cumulative attack reduce value

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public float maxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    public float health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                _health = 0;
                isAlive = false;
                // Optionally, perform death logic here
                animator.SetBool("isAlive", false);
                boxCollider.enabled = false;

            }
        }
    }


    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<CapsuleCollider2D>();
        poise = Maxpoise;
        health = _maxHealth;
        parryhealth = _maxHealth;

    }

    public void Hit(float damage, bool Stagger)
    {
        if (isAlive && !invincible)
        {

            invincible = true;
            timeSinceHit = 0; // Reset time since hit
            health -= damage;
            animator.SetTrigger(AnimationStrings.hit);
            if(Stagger)
            {
                animator.SetTrigger(AnimationStrings.StaggerHit);

            }



        }
    }

    private void Update()
    {
        if (invincible)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit >= invincibilityTime)
            {
                invincible = false; // End invincibility after invincibilityTime
            }
        }
    }


    private IEnumerator ResetPoiseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        resetpoise();
    }
    public void resetpoise()
    {
        poise = Maxpoise;
    }
    public void RecoverHealth(float amount)
    {
        if (isAlive)
        {
            _health += Mathf.RoundToInt(amount); // Convert the float amount to an int
            _health = Mathf.Clamp(_health, 0, _maxHealth); // Ensure health does not exceed maxHealth

            if (!isAlive && _health > 0) // If recovering from a state of 0 health
            {
                isAlive = true;
                animator.SetBool("isAlive", true);
                boxCollider.enabled = true;
            }
        }
    }

    private IEnumerator PoiseHit()
    {

        yield return new WaitForSeconds(counterDuration); // Wait for the specified duration
        poise = Maxpoise;
    }


}
