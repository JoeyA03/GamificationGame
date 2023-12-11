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
    private Ray pointerRay; //raycast for mouse position

    //dodging vaules
    private bool isDodging = false;
    public float dodgeSpeed = 30.0f;
    public float dodgeDuration = 0.5f;
    public float dodgeCooldown = 2.0f; // Adjust the cooldown time as needed
    private Vector3 dodgeDirection;
    public float dodgeWeightEffective = 1f;
    public float dodgeStaminaCost = 1f;
    float lastDodgeTime;
    Vector3 movement;

    //dodging vaules
    private bool isMeleeing = false;
    public float meleeSpeed = 30.0f;
    public float meleeDuration = 0.1f;
    public float meleeWeightEffective = 2f;
    public float meleeStaminaCost = 5f;


    //player variables for stamina calcs
    public float playerWeight = 1f;

    


    void Start()
    {
        speed = defaultSpeed;
        staminaWorkingValue = stamina.CheckStamina();
    }

    //
    void Update()   
    {
        MouseMovement();
        // // Get the mouse position in screen space.
        // Vector3 mousePositionScreen = Input.mousePosition;

        // // Convert the mouse position from screen space to world space.
        // Vector3 mousePositionWorld = Camera.main.ScreenPointToRay(new Vector3(mousePositionScreen.x, mousePositionScreen.y, Camera.main.transform.position.y));

        

        // // Check if the mouse is on the left or right side of the player
        // if (mousePositionWorld.x < transform.position.x)
        // {
        //     // Flip the sprite to face left
        //     GetComponent<SpriteRenderer>().flipX = true;
        // }
        // else
        // {
        //     // Flip the sprite to face right
        //     GetComponent<SpriteRenderer>().flipX = false;
        // }

        staminaWorkingValue = stamina.CheckStamina();
        // Calculate the direction from the player to the mouse.

        // Vector3 lookDirection = mousePositionWorld - transform.position;                                                                                                                
        // lookDirection.y = 0;                                                                                                                                              
        // if (lookDirection != Vector3.zero)                                                                                       
        // {                                                                                       
        //     transform.forward = lookDirection.normalized;                                                                                        
        // }                                                                                       

        // Player movement code (e.g., using WASD or arrow keys).
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement vector based on input.
        movement = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        // Translate the player based on the movement vector.
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        if (Input.GetKeyDown(KeyCode.Space) && !isDodging && Time.time - lastDodgeTime >= dodgeCooldown)
        {
            dodgeDirection = transform.forward; // Save the current look direction
            lastDodgeTime = Time.time;
            Stamina.UseStamina(dodgeStaminaCost * (dodgeWeightEffective*playerWeight));
            StartCoroutine(Dodge());
        }

        // rough Melee attack 
        if (Input.GetMouseButton(1) && !isMeleeing)
        {
            Stamina.UseStamina(meleeStaminaCost * (meleeWeightEffective*playerWeight));
            StartCoroutine(OnMelee());
        }
        // FLAME THROWA
        if (Input.GetMouseButton(0) && fuelSystem.IsFuelAvailable() && isDodging == false) // Change to Input.GetMouseButton(1) for right mouse button
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

    private void MouseMovement()
    {
        pointerRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray: pointerRay, hitInfo: out RaycastHit hit) && hit.collider)
        {
            Vector3 difference = hit.point - transform.position;
            difference.Normalize();

            float rotationZ = Mathf.Atan2(difference.z, difference.x) * Mathf.Rad2Deg - 90;             // Rremove -90 once a pivot is set to the chracter controller           

            this.gameObject.transform.GetChild(2).rotation = Quaternion.Euler(0, -rotationZ, 0);

            if (difference.x < transform.position.x)
            {
                // Flip the sprite to face left
                // gameObject.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
            else
            {
                // Flip the sprite to face right
                // gameObject.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            }

            // int rotation = ((((int)rotationZ / 45) + 1) * 45) - (45 / 2);                            // If we want to d snapped directions

        }
    }

    //TODO disable character movement when dodging 
    IEnumerator Dodge()
    {
        isDodging = true;
        speed = dodgeSpeed;

        // Apply the dodge movement for a certain duration
        float elapsed = 0.0f;
        Vector3 dodgeDirection = movement.normalized;
        while (elapsed < dodgeDuration)
        {
            transform.Translate(dodgeDirection  * dodgeSpeed * Time.deltaTime, Space.World);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset variables after the dodge
        speed = defaultSpeed;
        isDodging = false;
    }

    /*
    TODO 
    once animations are on, change logic to animation flags rather than corouutines
    Set up a list of already hit targets from melee attack to avoid multihit, or set up hitstun
    possibly not a while loop inside a courotine? 
    */

    //rough copy 
    IEnumerator OnMelee()
    {
        isMeleeing = true;
        // foreach(RaycastHit hit in Physics.CapsuleCastAll(this.gameObject.transform.GetChild(2), 2, transform.right))
        // {
        //     Debug.Log("Added " + hit.collider.name);
        // }

        float elapsed = 0.0f;
        while (elapsed < dodgeDuration)
        {
            elapsed += Time.deltaTime;
            Debug.Log("Attacking");
            foreach(RaycastHit hit in Physics.SphereCastAll(this.gameObject.transform.GetChild(2).GetChild(0).position, 1, transform.right))
            {
                Debug.Log("Added " + hit.collider.name);
            }
            yield return null;
        }

        yield return new WaitForSeconds(elapsed);
        isMeleeing = false;


    }




}
