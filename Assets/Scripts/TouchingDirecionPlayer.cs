using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class TouchingDirecionPlayer : MonoBehaviour
{


    public float groundDistance = 0.1f;
    public float wallDistance = 0.2f;
    public float ceillingDistance = 0.1f;
    
    public Transform wallCheck;
    public Transform groundcheck;
    public Transform ceilCheck;

    public LayerMask groundLayer;
    public LayerMask wallLayer;




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



    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(groundcheck.position, 0.2f, groundLayer);
        walled = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

        if (!IsGrounded && walled )
        {
            slide = true;
        }
        else
        {
            slide = false;
        }


    }
}



