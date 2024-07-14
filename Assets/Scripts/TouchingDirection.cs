using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchingDirection : MonoBehaviour
{
    public bool isPlayer;
    CapsuleCollider2D touchingcol;
    public float groundDistance = 0.1f;
    public float wallDistance = 0.2f;
    public float ceillingDistance = 0.1f;

    public Transform wallCheck;
    public Transform ceilCheck;
    public LayerMask groundLayer;

    public ContactFilter2D castFilter;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceillingHits = new RaycastHit2D[5];

    [SerializeField]private bool _isGrounded = true;
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
    [SerializeField] private bool _isOnWall = true;
    private Vector2 Wallcheckdirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    public bool IsOnWall
    {
        get { return _isOnWall; }
        private set
        {
            if (_isOnWall != value)
            {
                _isOnWall = value;
                animator.SetBool(AnimationStrings.isOnWall, value);
            }
        }
    }
    [SerializeField] private bool _isOnCeilling = true;

    public bool IsOnCeilling
    {
        get { return _isOnCeilling; }
        private set
        {
            if (_isOnCeilling != value)
            {
                _isOnCeilling = value;
                animator.SetBool(AnimationStrings.isOnCelling, value);
            }
        }
    }
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        touchingcol = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        // Check if the collider is touching the ground within the specified distance
        IsGrounded = touchingcol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;


        if (!isPlayer)
        {
            IsOnWall = touchingcol.Cast(Wallcheckdirection, castFilter, wallHits, wallDistance) > 0;
            IsOnCeilling = touchingcol.Cast(Vector2.up, castFilter, ceillingHits, ceillingDistance) > 0;

        }
        if(isPlayer)
        {
            IsOnWall = Physics2D.OverlapCircle(wallCheck.position, 0.5f, groundLayer);
            IsOnCeilling = Physics2D.OverlapCircle(ceilCheck.position, 0.5f, groundLayer);
        }

    }
}
