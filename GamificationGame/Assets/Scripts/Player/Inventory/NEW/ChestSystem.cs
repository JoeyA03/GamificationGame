using UnityEngine;

public class ChestSystem : MonoBehaviour
{
    public bool playerIn;
    public bool inChest;
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
            //hit.transform.GetComponent<Player>().InventorySet();s

            Debug.Log("hello");
        }

    }


    private void FixedUpdate()
    {
        playerIn = Physics.SphereCast(this.gameObject.transform.position, 3f, -transform.forward, out hit, 2f, playerLayerMask);

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
