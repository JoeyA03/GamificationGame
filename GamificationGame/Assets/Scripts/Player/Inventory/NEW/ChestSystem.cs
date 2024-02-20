using UnityEngine;

public class ChestSystem : MonoBehaviour
{
    public bool playerIn;
    public bool inChest;
    public float checkRadius;
    public LayerMask playerLayerMask;

    Collider boxCollider;
    RaycastHit hit;

    public GameObject chestUI;

    private void Start()
    {
        boxCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (playerIn && Input.GetKeyDown(KeyCode.E))
        {
            inChest = !inChest;
        }
    }
    private void FixedUpdate()
    {
        playerIn = Physics.SphereCast(this.gameObject.transform.position, checkRadius, -transform.forward, out hit, checkRadius, playerLayerMask);

        //if (Input.GetKeyDown(KeyCode.E)) 
        //{
        //    inChest = !inChest;
        //    //hit.transform.GetComponent<Player>().InventorySet();s
            
        //    Debug.Log("hello");
        //}
        //else 
        //{
        //    //chestUI.SetActive(false);
        //    Debug.Log("bye");
        //}

        chestUI.SetActive(inChest);
    }
}
