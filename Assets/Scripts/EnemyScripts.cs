using TMPro;
using UnityEngine;
using System.Collections;
using System;


[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]
public class EnemyScripts : MonoBehaviour
{
    Animator animator;
    public float walkSpeed = 5f;
    public float runSpeed;
    private DetectionZone attackZone;
    private CheckForPlayer checkZone;
    DamageAble damageAble;
    Rigidbody2D rb;
    TouchingDirection touchingDirection;
    public bool moveInBothAxes = false;
    private GameObject player;
    public int randomnumbers;
    private Transform target; // The target to move towards
    public float speed = 5f; // The speed at which to mov
    public float topspeed = 7.5f; // The speed at which to mov
    private bool _attackState = false;
    private Vector3 attackDirection;

    private GameObject trail;

    public GameObject objectToInstantiate; // The object to instantiate
    public float instantiationDistance = 1.0f; // Distance in front of the character to instantiate the object
    public Vector2 instantiationOffset = Vector2.zero; // Offset from the instantiation position

    public bool attackState
    {
        get { return _attackState; }
        private set
        {
            _attackState = value;
            animator.SetBool(AnimationStrings.AttackState, value);
        }
    }
    public bool _attacking = false;
    public bool attacking
    {
        get { return _attacking; }
        private set
        {
            _attacking = value;
            animator.SetBool(AnimationStrings.Attacking, value);
        }
    }

    private bool _moveToTarget = false;
    public bool moveToTarget
    {
        get { return _moveToTarget; }
        private set
        {
            _moveToTarget = value;
            animator.SetBool(AnimationStrings.Move, value);
        }
    }

    public bool turnable
    {
        get
        {
            return animator.GetBool(AnimationStrings.Turnable);
        }
    }
    public bool RunChargeAttack
    {
        get
        {
            return animator.GetBool(AnimationStrings.RunChargeAttack);
        }
    }
    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        touchingDirection = GetComponent<TouchingDirection>();
        animator = GetComponent<Animator>();
        damageAble = GetComponent<DamageAble>();

        Transform attackZoneTransform = transform.Find("AttackRange");
        Transform checkZoneTransform = transform.Find("CheckForPlayer");

        attackZone = attackZoneTransform.GetComponent<DetectionZone>();
        checkZone = checkZoneTransform.GetComponent<CheckForPlayer>();

        Transform trailObjectTransform = transform.Find("Trail");
        trail = trailObjectTransform.gameObject;
    }

    private void Update()
    {
        UpdateState();
        HandleMovement();

    }

    private void UpdateState()
    {
        attackState = attackZone.detectedcolliders.Count > 0;
        moveToTarget = checkZone.playerPos;
    }

    private void HandleMovement()
    {

        if (moveToTarget && !attackState)
        {

            Vector3 target = player.transform.position;
            Vector3 direction = (target - transform.position).normalized;
            if (canMove)
            {
                transform.position = transform.position + direction * speed * Time.deltaTime;
            }



            if (turnable)
            {
                if (direction.x > 0 && transform.localScale.x < 0)
                {
                    Flip();
                }
                else if (direction.x < 0 && transform.localScale.x > 0)
                {
                    Flip();
                }
            }


        }

        if (RunChargeAttack)
        {

            transform.position = new Vector3(transform.position.x + attackDirection.x * topspeed * Time.deltaTime, transform.position.y, transform.position.z);
            if (trail != null)
            {
                trail.gameObject.SetActive(true);
            }

        }
        else
        {
            trail.gameObject.SetActive(false);

        }
        if (attackState)
        {
            animator.SetBool(AnimationStrings.AttackState, true);
            animator.SetBool(AnimationStrings.Move, false);
            animator.SetInteger(AnimationStrings.Random, randomnumbers);
            ramdom();

        }


    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }


    public void ramdom()
    {
        if (!attacking)
        {
            randomnumbers = UnityEngine.Random.Range(0, 5); // Ensure it selects between 0, 1, and 2
            if (randomnumbers == 1)
            {
                attackDirection = (player.transform.position - transform.position).normalized;
            }


        }
    }

    void InstantiateInFacingDirection()
    {
        // Calculate the position to instantiate the object
        Vector2 instantiationPosition = (Vector2)transform.position + (Vector2)transform.right * instantiationDistance + instantiationOffset;

        // Instantiate the object
       // Instantiate(objectToInstantiate, instantiationPosition, Quaternion.identity);
    }
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;


    }
    public void attackReach(float amount)
    {
        rb.velocity = new Vector2(rb.velocity.x + amount, rb.velocity.y);

    }
}
