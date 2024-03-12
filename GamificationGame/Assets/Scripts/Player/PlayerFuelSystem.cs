using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerFuelSystem : MonoBehaviour
{
    public float maxFuel = 100.0f;         // Maximum fuel capacity
    public float fuelConsumptionRate = 5.0f;  // Rate at which fuel is consumed per second
    public float fuelRechargeRate = 10.0f;    // Rate at which fuel is recharged per second
    public float fuelRechargeDelay = 3.0f;   // Time in seconds before fuel starts recharging

    private float currentFuel;
    private Slider fuelSlider; // Reference to the fuel slider UI element
    private bool isUsingParticleSystem = false; // Flag to track particle system usage
    private float timeSinceLastUse = 0.0f; // Time since last particle system usage

    void Start()
    {
        currentFuel = maxFuel;

        // Find the Slider component in the Canvas and assign it to fuelSlider
        fuelSlider = GameObject.Find("Fuel").GetComponent<Slider>();
        UpdateFuelUI();
    }

    void Update()
    {
        // if (Input.GetMouseButton(0)) // Change to Input.GetMouseButton(1) for right mouse button
        // {
        //     isUsingParticleSystem = true;
        //     timeSinceLastUse = 0.0f;
        // }
        // else
        // {
        //     isUsingParticleSystem = false;
        // }

        if (isUsingParticleSystem)
        {
            // Consume fuel while the button is held down and there's fuel available
            if (currentFuel > 0)
            {
                currentFuel -= fuelConsumptionRate * Time.deltaTime;
            }
            else
            {
                // Stop particle system usage if fuel is depleted
                isUsingParticleSystem = false;
            }
        }
        else
        {
            // Start recharging fuel after the delay
            timeSinceLastUse += Time.deltaTime;

            if (timeSinceLastUse >= fuelRechargeDelay)
            {
                currentFuel += fuelRechargeRate * Time.deltaTime;

                // Ensure fuel doesn't exceed the maximum capacity
                currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
            }
        }

        UpdateFuelUI();
    }

    void UpdateFuelUI()
    {
        // Update the fuel slider value based on current fuel level
        if (fuelSlider != null)
        {
            fuelSlider.value = currentFuel;
        }
    }

    public void StartParticleSystem()
    {
        isUsingParticleSystem = true;
        timeSinceLastUse = 0.0f;
    }
    public void EndParticleSystem()
    {
        isUsingParticleSystem = false;
        // timeSinceLastUse = 0.0f;
    }

    public bool IsFuelAvailable()
    {
        return currentFuel > 0;
    }
}