using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Stamina : MonoBehaviour
{

    public Slider staminaSlider;
    public float maxStamina = 100.0f;         // Maximum stamina capacity
    public float staminaConsumptionRate = 5.0f;  // Rate at which stamina is consumed per second
    public float staminaRechargeRate = 10.0f;    // Rate at which stamina is recharged per second
    public float staminaRechargeDelay = 3.0f;   // Time in seconds before stamina starts recharging
    public static float currentStamina;
    private static float timeSinceLastUse = 0.0f; // Time since last particle system usage

    // Start is called before the first frame update
    void Start()
    {
        staminaSlider = GameObject.Find("Stamina").GetComponent<Slider>();
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        // Start recharging fuel after the delay
        timeSinceLastUse += Time.deltaTime;

        if (timeSinceLastUse >= staminaRechargeDelay)
        {
            currentStamina += staminaRechargeRate * Time.deltaTime;

            // Ensure fuel doesn't exceed the maximum capacity
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
        UpdateStaminaUI();
    }

    void UpdateStaminaUI()
    {
        // Update the stamina slider value based on current stamina level
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }

    public static void UseStamina(float amount)
    {
        currentStamina -= amount;
        timeSinceLastUse = 0.0f;
        
    }

    public float CheckStamina()
    {
        return currentStamina;
    }
}
