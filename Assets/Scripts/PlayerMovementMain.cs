using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(DamageAble), typeof(TouchingDirecionPlayer))]
public class PlayerMovementMain : MonoBehaviour
{
    Rigidbody2D rb;
    TouchingDirecionPlayer touchingdirection;
    GhostManager ghost;
    UIController UIcontrol;
    private float horizontal;
    public float speed = 8f;
    public float airspeed = 6f;
    public float jumpingPower = 12f;
    public bool isFacingRight = true;
    public VariableJoystick variableJoystick; // Ensure you have assigned this in the inspector.
    private Animator animator;
    public int CurrentMoveSpeed;

    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public LayerMask enemyhitbox;
    public float rolljumpstamina = 20f;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private float DashBufferTime = 0.2f;
    private float DashBufferCounter;
    private bool lockvelocity;
    private float originalGravityScale;
    public int doublejump = 1;
    private bool walljump;
    public bool isHorizontalInput;
    public bool test;
    public float wallSlidingSpeed;
    public bool allowAirDash;


    private bool canDash = true;
    private bool canAirDash = true;

    private bool isDashing;
    private float dashingPower = 16f;
    private float dashingTime = 0.25f;
    private float dashingCooldown = 3f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private void Start()
    {

    }

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Make sure your GameObject has an Animator component.
        touchingdirection = GetComponent<TouchingDirecionPlayer>();
        UIcontrol = GetComponent<UIController>();
        rb = GetComponent<Rigidbody2D>();
        // Set the layer masks to the appropriate layers
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        enemyhitbox = LayerMask.NameToLayer("EnemyHitbox");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        ghost = GetComponent<GhostManager>();
        lockvelocity = false;
        originalGravityScale = rb.gravityScale;
        
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (touchingdirection.IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            doublejump = 1;
            canAirDash = true;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (DashBufferCounter > 0f)
        {
            DashBufferCounter -= Time.deltaTime;
        }

        HandleJump();
        HandleDash();
        if (iswalled)
        {
            doublejump = 1;
            canAirDash = true;

        }
        if(isSlide)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

        }

    }

    private void FixedUpdate()
    {
        if (CanMove && isAlive )
        {
            if( !walljump && !Dashing)
            {
                float joystickInput = variableJoystick.Horizontal; // Make sure the joystick is setup correctly.
                float keyboardInput = Input.GetAxis("Horizontal");

                horizontal = joystickInput + keyboardInput;
                horizontal = Mathf.Clamp(horizontal, -1f, 1f);

                    if (!lockvelocity)
                    {
                        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                        if (Mathf.Abs(horizontal) > 0f && touchingdirection.IsGrounded)
                        {
                            animator.SetBool(AnimationStrings.Ismoving, true);
                        }
                        else
                        {
                            animator.SetBool(AnimationStrings.Ismoving, false);

                        }
                    }

            }

            if (!isFacingRight && horizontal > 0f || isFacingRight && horizontal < 0f)
            {
                if (isAlive && CanMove)
                {
                    Flip();
                }
            }

            if (!Dashing)
            {
                Physics2D.IgnoreLayerCollision(playerLayer, enemyhitbox, false);

            }
 
        }
        if(isSlide)
        {
            walljump = true;
        }
        else
        {
            walljump = false;
        }
        
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpBufferCounter = jumpBufferTime; // Set the buffer counter\

        }
        if (context.canceled)
        {
            if(doublejump >0 && rb.velocity.y > 0)
            {
                 rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }
    }

    private void HandleJump()
    {

        if (jumpBufferCounter > 0f && CanMove && isAlive&& !Dashing)
        {
            if (coyoteTimeCounter > 0f )
            {
                animator.SetTrigger(AnimationStrings.Jump);
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                coyoteTimeCounter = 0f;

            }
            else if (doublejump > 0)
            {
                if(isSlide)
                {
                    float directions = this.transform.localScale.x;
                    rb.velocity = new Vector2(jumpingPower * -directions, jumpingPower);
                    animator.SetTrigger(AnimationStrings.Jump);
                    doublejump--;
                    Invoke("Flip", 0.1f);

                }
                else
                {

                    animator.SetTrigger(AnimationStrings.Jump);
                    rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                    doublejump--; 
                }


            }

            jumpBufferCounter = 0f; 

        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(!allowAirDash)
            {
                    DashBufferCounter = DashBufferTime; // Set the roll buffer counter

            }
            else
            {
                if(canAirDash)
                {
                    StartCoroutine(AirDashIE());
                    animator.SetTrigger(AnimationStrings.Dash);
                }

            }

        }
    }

    private void HandleDash()
    {
        if (DashBufferCounter > 0f && isAlive && CanMove && !Dashing && stamina > rolljumpstamina && touchingdirection.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.Dash);
            DashBufferCounter = 0f; 

        }
    }
    private IEnumerator AirDashIE()
    {
        canAirDash = false;
        Dashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        ghost.makeghost = true;
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        Dashing = false;
        ghost.makeghost = false;
    }
    private IEnumerator DashIE()
    {
        canDash = false;
        Dashing = true;

        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        ghost.makeghost = true;
        yield return new WaitForSeconds(dashingTime);
        Dashing = false;
        ghost.makeghost = false;
        rb.gravityScale = 3f;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }


    public void LowGravity(bool value, float amount)
    {
        if (value)
        {
            rb.gravityScale = amount;
        }
        else
        {
            rb.gravityScale = originalGravityScale;
        }
    }


    [SerializeField] private bool _Dashing;
    public bool Dashing
    {
        get { return _Dashing; }
        private set
        {
            if (_Dashing != value)
            {
                _Dashing = value;
                animator.SetBool(AnimationStrings.Dashing, value);
            }
        }
    }
    public bool iswalled
    {
        get { return animator.GetBool(AnimationStrings.isOnWall); }
    }

 

    public bool isceiled
    {
        get { return animator.GetBool(AnimationStrings.isOnCelling); }
    }
    public bool isSlide
    {
        get { return animator.GetBool(AnimationStrings.wallSlide); }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool isAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    public float stamina
    {
        get
        {
            return UIcontrol.GetStamina();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
