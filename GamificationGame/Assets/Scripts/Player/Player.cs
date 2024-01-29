using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed;
    public float defaultSpeed = 15.0f;
    public Rigidbody rb;
    public ParticleSystem particleSys; // Reference to the particle system
    private bool isParticleSystemActive = false;
    public PlayerFuelSystem fuelSystem;
    public Stamina stamina; //Reference to the stamina
    private bool running = false;
    public bool isMoving = false;
    private float runSpeed = 18.0f;
    public float runStaminaWeight; 
    public float runStaminaCost; //base cost for stamina speed

    //player variables for stamina calcs

    public float playerHealth = 100;
    public float maxHP = 100;
    public GameObject healthSlider;
    private Slider sliderHealth;
    public GameObject canOne;
    public GameObject canTwo;
    public GameObject canThree;
    public int numOfCans = 1;
    public int maxCans = 3;

    public GameObject raycaster;

    

    private float staminaWorkingValue;
    private Ray pointerRay; //raycast for mouse position
    public LayerMask layerMask;  //layermask for mouse-over collision
    public LayerMask hitLayerMask;  //layermask for mouse-over collision

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

    public bool inInventory = false;                // Probably change this into a gamemanager.
    public GameObject inventoryUI;
    public GameObject inventorySystem;

    //player variables for stamina calcs
    public float playerWeight = 1f;

    //Player Animation Values
    public Animator playerAnimation;

    


    void Start()
    {
        speed = defaultSpeed;
        staminaWorkingValue = stamina.CheckStamina();
        rb = GetComponent<Rigidbody>();
        sliderHealth = healthSlider.GetComponentInChildren<Slider>();
        sliderHealth.value = playerHealth;
        UpdateCanister(0);
    }

    //
    void Update()   
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InventorySet();
        }
        

        if(inInventory)
        {
            return;
        }

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
       

        // Calculate the movement vector based on input.
        // 

        // Translate the player based on the movement vector.
        // transform.Translate(movement * speed * Time.deltaTime, Space.World);

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
            fuelSystem.StartParticleSystem();
            
            // Turn on the particle system if it's not already active
            if (!isParticleSystemActive)
            {
                particleSys.Play();
            }
            isParticleSystemActive = true;

            
        }
        else
        {
            fuelSystem.EndParticleSystem();
            
            isParticleSystemActive = false;
            // isParticleSystemActive = true;
            // Turn off the particle system if the mouse button is released
            if (!isParticleSystemActive)
            {
                particleSys.Stop();
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
        if (inInventory)
        {
            return;
        }

        if (running)
        {
            Stamina.UseStamina(runStaminaCost * runStaminaWeight);
        }
        


        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        movement = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        if(movement.sqrMagnitude != 0) 
        {
            isMoving = true;
        }
        else 
        {
            isMoving = false;
        }

        if(horizontalInput != 0 || verticalInput != 0)
        {
            playerAnimation.SetBool("isMoving", true);
        }
        else
        {
            playerAnimation.SetBool("isMoving", false);
        }
        
        rb.velocity = movement * speed;
    }

    private void MouseMovement()
    {
        pointerRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray: pointerRay, hitInfo: out RaycastHit hit, layerMask) && hit.collider)
        {
            Vector3 difference = hit.point - transform.position;
            difference.Normalize();

            float rotationZ = Mathf.Atan2(difference.z, difference.x) * Mathf.Rad2Deg - 90;             // Rremove -90 once a pivot is set to the chracter controller           

            this.gameObject.transform.GetChild(2).rotation = Quaternion.Euler(0, -rotationZ, 0);

            // Debug.Log(hit.point.x > transform.position.x);

            if (transform.position.x > hit.point.x)
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
            

            //
            // if(Input.GetMouseButton(0))                                                            
            // {
            //     //Debug Sphere to show size
            //     GameObject point1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //     point1.transform.position = hit.point;

            //     point1.AddComponent<DebugDestory>();
            //     Destroy(point1.GetComponent<SphereCollider>());
            // }

            // int rotation = ((((int)rotationZ / 45) + 1) * 45) - (45 / 2);                            // If we want to d snapped directions

        }
    }


    public void InventorySet()
    {
        inInventory = !inInventory;
        inventoryUI.SetActive(inInventory);
        inventorySystem.SetActive(inInventory);
        //inventoryUI.gameObject.transform.Find("Border").GetComponent<RectTransform>().anchoredPosition = new Vector2(-0, 0);
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

        //Debug Sphere to show hit location
        GameObject point1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        point1.transform.position = this.gameObject.transform.GetChild(2).GetChild(0).position;

        point1.AddComponent<DebugDestory>();
        Destroy(point1.GetComponent<SphereCollider>());

        float elapsed = 0.0f;
        while (elapsed < dodgeDuration)
        {
            elapsed += Time.deltaTime;
            Debug.Log("Attacking");

            //TODO CHANGE THIS  TO A RAYCAST IN THE FUTURE
            foreach(RaycastHit hit in Physics.SphereCastAll(this.gameObject.transform.GetChild(2).GetChild(0).position, 1, transform.up, 0f, hitLayerMask))
            {
                Debug.Log("Added " + hit.collider.name);
                if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-(hit.point - this.transform.position).normalized * 1, ForceMode.Impulse);
                    Debug.Log(hit.point);
                    Debug.Log("Hit Dumpster");
                }

            }
            yield return null;
        }

        yield return new WaitForSeconds(elapsed);
        isMeleeing = false;

    }

    public void UpdateHealth(int change)
    {
        if ((playerHealth + change) >= maxHP)
        {
            playerHealth = maxHP;
            sliderHealth.value = playerHealth;
        }
        else
        {
            playerHealth += change;
            sliderHealth.value = playerHealth;
        }
    }

    public void UpdateCanister(int change)
    {
        if ((numOfCans + change) >= maxCans)
        {
            numOfCans = maxCans;
        }
        else
        {
            numOfCans += change;
        }
        Color color;
        switch (numOfCans)
        {
            case 0:
                ColorUtility.TryParseHtmlString("#000000", out color);
                canOne.GetComponent<Image>().color = color;
                ColorUtility.TryParseHtmlString("#000000", out color);
                canTwo.GetComponent<Image>().color = color;
                ColorUtility.TryParseHtmlString("#000000", out color);
                canThree.GetComponent<Image>().color = color;
                break;
            case 1:
                ColorUtility.TryParseHtmlString("#FFFFFF", out color);
                canOne.GetComponent<Image>().color = color;
                ColorUtility.TryParseHtmlString("#000000", out color);
                canTwo.GetComponent<Image>().color = color;
                ColorUtility.TryParseHtmlString("#000000", out color);
                canThree.GetComponent<Image>().color = color;
                break;
            case 2:
                ColorUtility.TryParseHtmlString("#FFFFFF", out color);
                canOne.GetComponent<Image>().color = color;
                ColorUtility.TryParseHtmlString("#FFFFFF", out color);
                canTwo.GetComponent<Image>().color = color;
                ColorUtility.TryParseHtmlString("#000000", out color);
                canThree.GetComponent<Image>().color = color;
                break;
            case 3:
                ColorUtility.TryParseHtmlString("#FFFFFF", out color);
                canOne.GetComponent<Image>().color = color;
                ColorUtility.TryParseHtmlString("#FFFFFF", out color);
                canTwo.GetComponent<Image>().color = color;
                ColorUtility.TryParseHtmlString("#FFFFFF", out color);
                canThree.GetComponent<Image>().color = color;
                break;
        }
    }



}
