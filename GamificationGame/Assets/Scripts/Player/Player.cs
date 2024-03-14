using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStats playerStats;
    public float speed;
    private float defaultSpeed;
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
    public StimuliSystem stimuli;   // Stimuli Effect
     

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

    public bool inInventory = false;    // Probably change this into a gamemanager.
    public bool inChest = false;
    public GameObject inventoryUI;
    public GameObject chestUI;
    public GameObject inventorySystem;

    //Dodging values
    private bool isBlocking = false;



    //player variables for stamina calcs
    public float playerWeight = 1f;

    //Player Animation Values
    public Animator playerAnimation;
    
    void Start()
    {
        
        staminaWorkingValue = stamina.CheckStamina();
        rb = GetComponent<Rigidbody>();
        
        UpdateCanister(0);
        Init();
    }

    private void Init() 
    {
        //Stats
        maxHP = playerStats.maxHealth;
        defaultSpeed = playerStats.baseSpeed;
        //defaultSpeed = playerStats.baseSpeed;

        speed = defaultSpeed;

        //UI
        sliderHealth = healthSlider.GetComponentInChildren<Slider>();
        sliderHealth.value = playerHealth;
    }


    void Update()   
    {
        /// E for interact / Inventory
        if (Input.GetKeyDown(KeyCode.E))
        {
            InventorySet();
        }


        // If the player is in inventory, remove access to player functions
        if (inInventory) return;

        
        

        staminaWorkingValue = stamina.CheckStamina();
        /// Space for Dodge
        // Rough Dash 
        if (Input.GetKeyDown(KeyCode.Space) && !isDodging && Time.time - lastDodgeTime >= dodgeCooldown)
        {
            dodgeDirection = transform.forward; // Save the current look direction
            lastDodgeTime = Time.time;
            Stamina.UseStamina(dodgeStaminaCost * (dodgeWeightEffective * playerWeight));
            StartCoroutine(Dodge());
        }

        /// V for Melee
        // rough Melee attack 
        if (Input.GetKeyDown(KeyCode.V) && !isMeleeing)
        {
            Stamina.UseStamina(meleeStaminaCost * (meleeWeightEffective * playerWeight));
            StartCoroutine(OnMelee());
        }



        /// Right Click  for Blocking
        if (Input.GetMouseButton(1) && !isBlocking)
        {

            
        }

        /// Left Click Shooting
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

        // Mouse movement.
        MouseMovement();
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
                

        }
    }


    public void InventorySet()
    {  
        inInventory = !inInventory;

        inventoryUI.SetActive(inInventory);
        inventorySystem.SetActive(inInventory);

        if(inChest)
            chestUI.SetActive(inInventory);     // Open Chest inventory if this bool is on
        rb.velocity = Vector3.zero;     // Make sure that the player is not moving when in invetory;
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
            foreach(RaycastHit hit in Physics.SphereCastAll(this.gameObject.transform.GetChild(2).GetChild(0).position, 1, transform.right, 1f, hitLayerMask))
            {
                Debug.Log("Added " + hit.collider.name);
                if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce((hit.transform.position - this.transform.position).normalized * .5f, ForceMode.Impulse);
                    //Debug.Log(hit.point);
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
