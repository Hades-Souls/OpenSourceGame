using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider staminaSlider;  // Reference to the slider UI
    public Slider easestaminaSlider;  // Reference to the slider UI    public Slider staminaSlider;  // Reference to the slider UI
    public Slider HealthSlider;  // Reference to the slider UI    public Slider staminaSlider;  // Reference to the slider UI
    public Slider easeHealthSlider;  // Reference to the slider UI
    public Slider recoverableHealthslider;
    PlayerDamagable playerDamagable;

    public float maxStamina = 100f;
    public float stamina = 0f;

    public float maxHealth = 100f;
    public float health = 0f;
    public float increaseRate = 5f;  // Stamina increase rate per second
    private float lerpSpeed = 0.005f;
    private float recoverableHealthvalue;
    AttackChain attackchain;
    void Start()
    {

        // Find the slider by name


        // Ensure the slider is not null
        if (staminaSlider == null)
        {
            Debug.LogError("Slider named 'Stamina' not found or Slider component missing.");
        }       
        if (easestaminaSlider == null)
        {
            Debug.LogError("Slider named 'EaseStamina' not found or Slider component missing.");
        }
               if (HealthSlider == null)
        {
            Debug.LogError("Slider named 'Stamina' not found or Slider component missing.");
        }       
        if (easeHealthSlider == null)
        {
            Debug.LogError("Slider named 'EaseStamina' not found or Slider component missing.");
        }

    }

    private void Awake()
    {
        staminaSlider = GameObject.Find("Stamina").GetComponent<Slider>();
        easestaminaSlider = GameObject.Find("EaseStamina").GetComponent<Slider>();
        easeHealthSlider = GameObject.Find("EaseHealth").GetComponent<Slider>();
        HealthSlider = GameObject.Find("Health").GetComponent<Slider>();
        recoverableHealthslider = GameObject.Find("RecoverableHealth").GetComponent<Slider>();
        attackchain = GetComponent<AttackChain>();
        // Ensure the DamageAble component is properly assigned
        playerDamagable = GetComponent<PlayerDamagable>(); // This assumes the DamageAble component is attached to the same GameObject as UIController

        stamina = maxStamina;
        health = maxHealth;

        UpdateStaminaSlider();
        UpdateHealthSlider();
        UpdaterecoverableHealthslider();

        easestaminaSlider.value = maxStamina;
        easeHealthSlider.value = maxHealth;
        maxHealth = playerDamagable.maxHealth;

    }
    void Update()
    {
        // Increase stamina over time
        if (stamina <= maxStamina)
        {
            stamina += increaseRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);  // Clamp the value between 0 and maxStamina
            UpdateStaminaSlider();  // Update the slider value
        }
        if(playerDamagable.health <= playerDamagable.maxHealth)
        {
            UpdateHealthSlider();  // Update the slider value
        }
        easestaminaSlider.value = Mathf.Lerp(easestaminaSlider.value, stamina, lerpSpeed);
        easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, playerDamagable.health, lerpSpeed);

        UpdaterecoverableHealthslider();



    }
    public void RecoverAfterAttack(bool charge, float amount)
    {
        float recoverAmount = 0f;// Amount to recover
        float RecoverPertrigger = playerDamagable.totalAttackReduce /5;
        if (!charge)
        {
            recoverAmount = RecoverPertrigger;
            Debug.Log(recoverAmount);



        }
        else
        {
            if(attackchain.hold >= 1.5f)
            {
                recoverAmount = playerDamagable.totalAttackReduce;
                Debug.Log(recoverAmount);

            }
            else
            {
                recoverAmount = RecoverPertrigger;

            }

            attackchain.hold = 0f;


        }
        playerDamagable.RecoverHealth(recoverAmount);
        playerDamagable.totalAttackReduce -= recoverAmount;
;
    }
    public float GetStamina()
    {
        return stamina;
    }
 
    // Public method to reduce the stamina value
    public void ReduceStamina(float amount)
    {
        stamina -= amount;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);  // Clamp the value between 0 and maxStamina
        UpdateStaminaSlider();  // Update the slider value
    }

    // Public method to set the stamina value
    public void SetStamina(float value)
    {
        stamina = Mathf.Clamp(value, 0, maxStamina);  // Clamp the value between 0 and maxStamina
        UpdateStaminaSlider();  // Update the slider value
    }

    // Private method to update the slider value
    private void UpdateStaminaSlider()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = stamina;
        }
        else
        {
        }
    }    
    private void UpdateHealthSlider()
    {
        if (HealthSlider != null)
        {
            HealthSlider.value = playerDamagable.health;
        }
        else
        {
        }
    }   
    private void UpdaterecoverableHealthslider()
    {
        if (recoverableHealthslider != null)
        {
            recoverableHealthslider.value = playerDamagable.health + playerDamagable.totalAttackReduce;
        }
 
    }
 
    public void damageTaken()
    {
        float amount = 0;
        amount = health - easeHealthSlider.value;
    }
}
