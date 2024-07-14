using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackChain : MonoBehaviour
{
    public float holdThreshold = 1f; // Threshold to determine strong attack
    private float increaseRate = 1f;

    private Animator animator;
    private PlayerMovementMain player;
    private PlayerInput playerInput; // Reference to the PlayerInput component
    private InputAction normalAttack;
    private InputAction StrongAttack;
    private InputAction ParryInput;

    TouchingDirecionPlayer touchingdirection;

    private GameObject strongattack;
    private attack attack;
    private float holdTime = 0f; // Track how long the button is held
    public float hold = 0f;

    private float parryTime = 1f;
    public float parry = 0f;

    public int JumpCount = 2;
    private float AirttackCount;


    private void Awake()
    {
        strongattack = GameObject.Find("StrongAttack");
        attack = strongattack.GetComponent<attack>();
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
            AirttackCount = 2;
        }


    }
    private void OnNormalAttackPerfomed(InputAction.CallbackContext context)
    {



            animator.SetTrigger(AnimationStrings.Attack);



    }


    private void OnMoveActionCanceled(InputAction.CallbackContext context)
    {

        hold = holdTime;
        animator.SetBool(AnimationStrings.Hold, false);
        animator.SetTrigger(AnimationStrings.StrongAttack);
        float attackincrease = attack.attackdamage * holdTime * increaseRate;
        float maxAllowedIncrease = attack.attackdamage * 2f;
        // Limit the increase to the maximum allowed increase
        attackincrease = Mathf.Min(attackincrease, maxAllowedIncrease);
        float tempAttackDamage = attack.attackdamage;
        attack.attackdamage = attackincrease;
        attack.attackdamage = tempAttackDamage; // Reset the attack damage after use
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