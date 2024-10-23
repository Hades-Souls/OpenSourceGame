using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackChain : MonoBehaviour
{
    public float holdThreshold = 1f; // Threshold to determine strong attack

    private Animator animator;
    private PlayerMovementMain player;
    private PlayerInput playerInput; // Reference to the PlayerInput component
    private InputAction normalAttack;
    private InputAction StrongAttack;
    private InputAction ParryInput;
    TouchingDirecionPlayer touchingdirection;
    public int attackCount;
    private GameObject strongattack;
    private float holdTime = 0f; // Track how long the button is held
    public float hold = 0f;
    private float parryTime = 1f;
    public float parry = 0f;
    public int JumpCount = 2;


    private void Awake()
    {
        strongattack = GameObject.Find("StrongAttack");
        player = GetComponent<PlayerMovementMain>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>(); // Ensure there's a PlayerInput component attached to the same GameObject
        normalAttack = playerInput.actions["Attack"]; // "Hit" should be replaced with the name of your actual input action
        StrongAttack = playerInput.actions["StrongAttack"]; // "Hit" should be replaced with the name of your actual input action
        ParryInput = playerInput.actions["Parry"]; // "Hit" should be replaced with the name of your actual input action
        ParryInput.canceled += ParryOnMoveActionCanceled;
        ParryInput.performed += OnParryAttackPerformed;
        StrongAttack.canceled += OnMoveActionCanceled;
        normalAttack.performed += OnNormalAttackPerfomed;
        
        touchingdirection = GetComponent<TouchingDirecionPlayer>();


    }

    void Update()
    {
        if (StrongAttack.IsPressed())
        {
            if (holdTime <= 2f)
            {
                holdTime += Time.deltaTime; // Update holdTime continuously while StrongAttack is pressed
            }
            animator.SetBool(AnimationStrings.Hold,true);
            if (ishit)
            {

                holdTime = 0f;
                hold = holdTime;
            }
        }
        if (ParryInput.IsPressed())
        {
            if (parryTime <= 1f && parryTime >= 0.25f)
            {
                parryTime -= Time.deltaTime; // Update holdTime continuously while StrongAttack is pressed
            }
            animator.SetBool(AnimationStrings.Parry,true);
            parry = parryTime;

        }
        if(touchingdirection.IsGrounded)
        {
            animator.SetBool(AnimationStrings.AirAttack, true);
        }

  

    }
    private void OnNormalAttackPerfomed(InputAction.CallbackContext context)
    {
      if(context.performed && touchingdirection.IsGrounded)
        {
            if (animator != null)
            {

                animator.SetTrigger(AnimationStrings.Attack);
            }
        }
      
      else
        {
            animator.SetTrigger(AnimationStrings.AirAttackTrigger);

        }
 



    }
   


    private void OnMoveActionCanceled(InputAction.CallbackContext context)
    {

        hold = holdTime;
        animator.SetBool(AnimationStrings.Hold, false);
        animator.SetTrigger(AnimationStrings.StrongAttack);
        // Limit the increase to the maximum allowed increase
        holdTime = 0f;

    }
    
    private void ParryOnMoveActionCanceled(InputAction.CallbackContext context)
    {
        animator.SetBool(AnimationStrings.Parry, false);
    }
    private void OnParryAttackPerformed(InputAction.CallbackContext context)
    {
        animator.SetBool(AnimationStrings.Parry, true);
        parryTime = 1f;
    }



    public bool AirAttack
    {
        get
        {
            return animator.GetBool(AnimationStrings.AirAttack);
        }
    }

    public bool Isparry
    {
        get
        {
            return animator.GetBool(AnimationStrings.Parry);
        }
    }    
    private bool ishit
    {
        get
        {
            return animator.GetBool(AnimationStrings.hit);
        }
    }



}