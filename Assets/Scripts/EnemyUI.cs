using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    // Reference to the HealthSlider
    public Slider HealthSlider;  // Reference to the slider UI    public Slider staminaSlider;  // Reference to the slider UI
    public Slider easeHealthSlider;  // Reference to the slider UI

    // Reference to the health values
    DamageAble damageAble;


    public float maxHealth = 100f;
    public float health = 0f;
    private float lerpSpeed = 0.005f;

    private void Start()
    {
        if (HealthSlider == null)
        {
            Debug.LogError("Slider named 'Stamina' not found or Slider component missing.");
        }
        if (easeHealthSlider == null)
        {
            Debug.LogError("Slider named 'EaseStamina' not found or Slider component missing.");
        }
    }
    void Awake()
    {
        // Get the DamageAble component

        // Find the Canvas Object (assuming it's a child of the enemy)
        Canvas canvas = GetComponentInChildren<Canvas>();

        if (canvas != null)
        {
            // Find the HealthSlider component within the Canvas Object
            Transform healthSliderParent = canvas.transform.Find("HealthSlider");
            if (healthSliderParent != null)
            {
                HealthSlider = healthSliderParent.Find("EnemyHealth").GetComponent<Slider>();
            }
            else
            {
                Debug.LogError("HealthSlider parent object not found in the Canvas.");
            }

            // Find the EaseHealthSlider component within the Canvas Object
            if (healthSliderParent != null)
            {
                easeHealthSlider = healthSliderParent.Find("EnemyEaseHealth").GetComponent<Slider>();
            }
            else
            {
                Debug.LogError("HealthSlider parent object not found in the Canvas.");
            }
        }
        else
        {
            Debug.LogError("Canvas not found as a child of the enemy.");
        }
        // Ensure the DamageAble component is properly assigned
        damageAble = GetComponent<DamageAble>(); // This assumes the DamageAble component is attached to the same GameObject as UIController

        health = maxHealth;
        UpdateHealthSlider();
        easeHealthSlider.value = maxHealth;
        maxHealth = damageAble.maxHealth;

    }
    private void Update()
    {
        if (damageAble.health <= damageAble.maxHealth)
        {
            UpdateHealthSlider();  // Update the slider value

        }
        easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, damageAble.health, lerpSpeed);

    }

    private void UpdateHealthSlider()
    {
        if (HealthSlider != null)
        {
            HealthSlider.value = damageAble.health;

        }
        else
        {
        }
    }
}
