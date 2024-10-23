using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class TouchingDirecionPlayer : MonoBehaviour
{

    public float groundDistance = 0.1f;
    public float wallDistance = 0.2f;
    public float ceillingDistance = 0.1f;
    
    public Vector2 bonuspos;
    public bool Climbledge;
    public Transform groundcheck;
    private float horizontal;
    public bool climb;
    public LayerMask groundLayer;
    PlayerMovementMain player;
    Rigidbody2D rb;
    public float wallSlidingSpeed;
    public float FastwallSlidingSpeed;

    public Transform ledgeCheck;
    public float characterHeight;
    public float characterWide;
    private bool isClimbing;

    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    private DetectForLedge detectledge;
    private BoxCollider2D ledgeDetector;

    public Transform ceilingCheck; // Reference to the ceiling check position

    public LayerMask wallLayer; // LayerMask to specify the wall layer
    public float detectionDistance = 0.1f; // Distance to check for the wall

    private CapsuleCollider2D capsuleCollider;
    RaycastHit2D[] groundhits = new RaycastHit2D[1];

    public Vector2 boxSize = new Vector2(1f, 1f); // Size of the box
    public float boxAngle = 0f; // Angle of the box (rotation)
    public Color gizmoColor = Color.red; // Color of the gizmo to visualize the cast
    public Transform wallcheck; // Transform to specify the wall check position
    public bool isWalled;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerMovementMain>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        detectledge = GetComponentInChildren<DetectForLedge>();
        ledgeDetector = detectledge.GetComponent<BoxCollider2D>();
        ledgeDetector.enabled = false;

    }

    [SerializeField] private bool _isGrounded = true;
    public bool IsGrounded
    {
        get { return _isGrounded; }
        private set
        {
            if (_isGrounded != value)
            {
                _isGrounded = value;
                animator.SetBool(AnimationStrings.Grounded, value);
            }
        }
    }
    [SerializeField] private bool _walled;
    public bool walled
    {
        get { return _walled; }
        private set
        {
            if (_walled != value)
            {
                _walled = value;
                animator.SetBool(AnimationStrings.isOnWall, value);
            }
        }
    }
    [SerializeField] private bool _groundRight;
    public bool groundRight
    {
        get { return _groundRight; }
        private set
        {
            if (_groundRight != value)
            {
                _groundRight = value;
            }
        }
    }
    [SerializeField] private bool _slide;
    public bool slide
    {
        get { return _slide; }
        private set
        {
            if (_slide != value)
            {
                _slide = value;
                animator.SetBool(AnimationStrings.wallSlide, value);
            }
        }
    }

    [SerializeField] private bool _ceilCheck;
    public bool ceilCheck
    {
        get { return _ceilCheck; }
        private set
        {
            if (_ceilCheck != value)
            {
                _ceilCheck = value;
            }
        }
    }

    Animator animator;


    private void FixedUpdate()
    {
        if(checkInput.wasinteractPress)
        {
            Debug.Log("e pressed");
        }
        if (!IsGrounded)
        {
            ledgeDetector.enabled = true;
        }
        else
        {
            ledgeDetector.enabled = false;

        }
        


        horizontal = player.Horizontal;
        IsGrounded = Physics2D.OverlapCircle(groundcheck.position, 0.2f, groundLayer);
        walled = Physics2D.OverlapCircle(Vector2.right, 0.1f, wallLayer);

        isWalled = Physics2D.OverlapBox(wallcheck.position, boxSize, boxAngle, wallLayer);
        if (isWalled)
        {
            Debug.Log("Collider detected within the box.");
        }


        if (groundRight)
        {
            Debug.Log("groundRight");

        }


        ceilCheck = Physics2D.OverlapCircle(ceilingCheck.position,wallDistance, groundLayer);
        if (ceilCheck && IsGrounded)
        {

            animator.SetBool(AnimationStrings.Crouch, true);

        }
        else
        {
            animator.SetBool(AnimationStrings.Crouch, false);

        }
        if (player.canWallSlide)
        {
            if (!IsGrounded && walled && !isClimbing)
            {
                slide = true;
                if (horizontal != 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
                }
                if (horizontal == 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -FastwallSlidingSpeed, float.MaxValue));
                }


            }
            else
            {
                slide = false;
            }
        }
        else
        {
            if(walled)
            {
                player.lockvelocity = true;
            }
            else
            {
                player.lockvelocity = false;

            }
        }
       
     

        if (!isClimbing && detectledge.AreAnyCollidersDetected())
        {
            HandleLedgeCollision(detectledge.detectedcolliders[0].transform.position);
        }

    }
    void HandleLedgeCollision(Vector2 ledgePosition)
    {
        ledgePos1 = ledgePosition;
        float direction = transform.localScale.x;
        if (direction > 0)
        {
            ledgePos2 = new Vector2(ledgePos1.x + characterWide, ledgePos1.y + characterHeight);

        }
        else
        {
            // Climbing to the left
            ledgePos2 = new Vector2(ledgePos1.x - characterWide, ledgePos1.y + characterHeight);
        }
        StartClimb();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Draw ground check
        if (groundcheck != null)
        {
            Gizmos.DrawWireSphere(groundcheck.position, groundDistance);
        }
 
            if (wallcheck != null)
            {
                Gizmos.color = gizmoColor;
                // Draw the wireframe of the box
                Gizmos.DrawWireCube(wallcheck.position, boxSize);
            }
    



    }
    void StartClimb()
    {
        isClimbing = true;
        rb.gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Animator>().SetTrigger("Climb");
        transform.position = ledgePos1;

    }

    public void OnClimbAnimationEnd()
    {
        transform.position = ledgePos2;
        rb.gravityScale = 3;
        isClimbing = false;
    }

}



