using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    
    public float health = 100f;
    public Slider healthSlider;

    void Start()
    {
        // Set the slider's maximum value at the start and initialize its value.
        if (healthSlider != null)
        {
            healthSlider.maxValue = 100f; // Assuming 100 is the max health
            healthSlider.value = health;
        }
    }

    public void ReduceHealth(float amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0); // Ensure health does not go below 0
        UpdateHealthSlider();

        if (health <= 0)
        {
            Die();
        }
    }

    public void IncreaseHealth(float amount)
    {
        health += amount;
        health = Mathf.Min(health, healthSlider.maxValue); // Ensure health does not exceed max
        UpdateHealthSlider();
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died.");
        Destroy(gameObject);
    }

    void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }
    }
}
