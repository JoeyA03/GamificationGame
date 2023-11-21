using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed;
    public float defaultSpeed = 15.0f;
    public ParticleSystem particleSys; // Reference to the particle system
    private bool isParticleSystemActive = false;
    public PlayerFuelSystem fuelSystem;
    public Stamina stamina; //Reference to the stamina
    private bool running = false;
    private float runSpeed = 18.0f;
    public float runStaminaWeight; 
    public float runStaminaCost; //base cost for stamina speed
    private float staminaWorkingValue;

    void Start()
    {
        speed = defaultSpeed;
        staminaWorkingValue = stamina.CheckStamina();
    }

    void Update()
    {
        staminaWorkingValue = stamina.CheckStamina();
        // Get the mouse position in screen space.
        Vector3 mousePositionScreen = Input.mousePosition;

        // Convert the mouse position from screen space to world space.
        Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePositionScreen.x, mousePositionScreen.y, Camera.main.transform.position.y));

        // Calculate the direction from the player to the mouse.
        Vector3 lookDirection = mousePositionWorld - transform.position;

        // Ensure the player doesn't tilt up or down (keeping them upright).
        lookDirection.y = 0;

        // Rotate the player to look at the mouse.
        if (lookDirection != Vector3.zero)
        {
            transform.forward = lookDirection.normalized;
        }

        // Player movement code (e.g., using WASD or arrow keys).
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement vector based on input.
        Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        // Translate the player based on the movement vector.
        transform.Translate(movement * speed * Time.deltaTime, Space.World);


        // FLAME THROWA
        if (Input.GetMouseButton(0) && fuelSystem.IsFuelAvailable()) // Change to Input.GetMouseButton(1) for right mouse button
        {
            // Turn on the particle system if it's not already active
            if (!isParticleSystemActive)
            {
                particleSys.Play();
                isParticleSystemActive = true;
            }
        }
        else
        {
            // Turn off the particle system if the mouse button is released
            if (isParticleSystemActive)
            {
                particleSys.Stop();
                isParticleSystemActive = false;
            }
        }

        //ALL STAMINA BASED THINGS SHOULD BE DONE UNDER HERE ---- SO WE ONLY NEED TO STAMINA CHECK ONCE PER UPDATE
        if( staminaWorkingValue>= 0.5f)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                running = true;
                speed = runSpeed;
            }
        }
        else
        {
            running = false;
            speed = defaultSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            Debug.Log("lifted");
            running = false;
            speed = defaultSpeed;
        }

    }

    void FixedUpdate()
    {
        if(running)
        {
            Stamina.UseStamina(runStaminaCost * runStaminaWeight);
        }
    }

}
