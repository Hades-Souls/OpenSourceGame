using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerDamagable), typeof(TouchingDirecionPlayer))]
public class PlayerMovementMain : MonoBehaviour
{
    Rigidbody2D rb;
    TouchingDirecionPlayer touchingdirection;
    UIController UIcontrol;
    GhostTrail ghosttrail;

    [Header("Value")]
    public float speed = 8f;
    public float airspeed = 6f;
    public float jumpingPower = 12f;
    public int doublejump = 1;
    public float wallSlidingSpeed;
    public int jumpAmount;
    public float HorizontalInput;
    public float dashingCooldown = 3f;
    public float jumpBufferTime = 0.2f;
    public float coyoteTime = 5f;
    public float Horizontal;

    [Header("Bool Value")]
    public bool test;
    public bool allowAirDash;
    public bool Doublejump;
    public bool Candropdown;
    public bool canWallSlide;
    public bool lockvelocity;

    private bool canDash = true;
    private bool canAirDash = true;
    private bool isDashing;

    private float horizontal;

    public VariableJoystick variableJoystick; // Ensure you have assigned this in the inspector.
    private Animator animator;
    public int CurrentMoveSpeed;
    private bool isFacingRight = true;
    public float dropDelay = 2f; // Delay before re-enabling the platform collision
    private PlayerInput playerInput; // Reference to the PlayerInput component
    private InputAction JumpInput;

    private Collider2D playerCollider;

    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public LayerMask enemyhitbox;

    private GameObject currentOneWayPlatform;

    public float rolljumpstamina = 20f;

    private float coyoteTimeCounter;

    private float jumpBufferCounter;

    private float DashBufferTime = 0.2f;
    private float DashBufferCounter;
    private float originalGravityScale;
    private bool walljump;

    private float dashingPower = 16f;
    private float dashingTime = 0.25f;



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
        playerCollider = GetComponent<Collider2D>();
        playerInput = GetComponent<PlayerInput>();
        // Set the layer masks to the appropriate layers
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        enemyhitbox = LayerMask.NameToLayer("EnemyHitbox");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        lockvelocity = false;
        originalGravityScale = rb.gravityScale;
        JumpInput = playerInput.actions["Jump"]; // "Hit" should be replaced with the name of your actual input action
        ghosttrail = GetComponent<GhostTrail>();
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
            jumpAmount = 0;
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

        if(AirAttack)
        {
            rb.velocity = new Vector2(0, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

    }

    private void FixedUpdate()
    {
 
        if (CanMove && isAlive && !AirAttack )
        {

                float joystickInput = variableJoystick.Horizontal; // Make sure the joystick is setup correctly.
                float keyboardInput = Input.GetAxis("Horizontal");
                horizontal = joystickInput + keyboardInput;
                
                horizontal = Mathf.Clamp(horizontal, -1f, 1f);

                if (!lockvelocity &&!walljump && !Dashing && !isCrouch)
                    {
                        if(!touchingdirection.groundRight)
                            {
                                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                            }
                if (Mathf.Abs(horizontal) > 0f && touchingdirection.IsGrounded && !isCrouch)
                        {
                            animator.SetBool(AnimationStrings.Ismoving, true);
                        }
                        else
                        {
                            animator.SetBool(AnimationStrings.Ismoving, false);

                        }
                    }

            

            if (!isFacingRight && horizontal > 0f || isFacingRight && horizontal < 0f)
            {
                if (!walljump)
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
        Horizontal = horizontal;
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpBufferCounter = jumpBufferTime; // Set the buffer counter\

        }

        if (context.canceled)
        {
            if(rb.velocity.y > 0)
            {
                 rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
            coyoteTimeCounter = 0f;

        }

    }

    private void HandleJump()
    {

        if (jumpBufferCounter > 0f && CanMove && isAlive&& !Dashing)
        {

            if(coyoteTimeCounter > 0.1f)
            {
                animator.SetTrigger(AnimationStrings.Jump);
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
            }
            else if (doublejump > 0 && !touchingdirection.IsGrounded )
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
                    if(Doublejump)
                    {
                        animator.SetTrigger(AnimationStrings.Jump);
                        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                        doublejump--;
                    }

                }

                
            }

            jumpBufferCounter = 0f; 

        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!allowAirDash)
            {
                DashBufferCounter = DashBufferTime; // Set the dash buffer counter
                                                 // Perform ground dash logic here if needed
            }
            else if (allowAirDash && canAirDash)
            {
                StartCoroutine(AirDashIE());
                animator.SetTrigger(AnimationStrings.Dash);
            }
        }
    }
    public void dropdown(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());

            }

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("platform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("platform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    private void HandleDash()
    {
        if (DashBufferCounter > 0f && isAlive && CanMove && !Dashing && canDash&& stamina > rolljumpstamina && touchingdirection.IsGrounded )
        {
            animator.SetTrigger(AnimationStrings.Dash);
            StartCoroutine(DashIE());

        }

    }
    private IEnumerator AirDashIE()
    {
        canAirDash = false;
        Dashing = true;
        ghosttrail.canCreateGhosts = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        ghosttrail.canCreateGhosts = false;

        Dashing = false;
    }
    private IEnumerator DashIE()
    {
        canDash = false;
        Dashing = true;
        ghosttrail.canCreateGhosts = true;

        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        Dashing = false;
        rb.gravityScale = 3f;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        ghosttrail.canCreateGhosts = false;

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
    public bool canFlip
    {
        get { return animator.GetBool(AnimationStrings.canFlip); }
    }

    public bool iswalled
    {
        get { return animator.GetBool(AnimationStrings.isOnWall); }
    }
 

    public bool AirAttack
    {
        get { return animator.GetBool(AnimationStrings.BoolAirAttack); }
    }
    public bool isCrouch
    {
        get { return animator.GetBool(AnimationStrings.Crouch); }
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
