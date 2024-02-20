using UnityEngine;

public class ChestSystem : MonoBehaviour
{
    public bool playerIn;
    public bool inChest;
    public float checkRadius;
    public LayerMask playerLayerMask;
    public ItemGrid ig;

    Collider boxCollider;
    RaycastHit hit;

    public GameObject chestUI;

    private void Start()
    {
        boxCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("player in chest");

        if (collision.TryGetComponent<Player>(out Player p)) 
        {
            p.inChest = true;
        }

        if (collision.transform.GetChild(3).GetChild(5) != null) 
        {
            Debug.Log("does exist");
            Transform cinv = collision.transform.GetChild(3).GetChild(5);

            cinv.GetComponent<ItemGrid>().chestOpen();

            cinv.GetComponent<ItemGrid>().sizeWidth = ig.sizeWidth;
            cinv.GetComponent<ItemGrid>().sizeHeight = ig.sizeHeight;
            cinv.GetComponent<ItemGrid>().itemPrefab = ig.itemPrefab;
            cinv.GetComponent<ItemGrid>().items = ig.items;
        }

    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent<Player>(out Player p))
        {
            p.inChest = false;
            Debug.Log("player out");
        }

        if (collision.transform.GetChild(3).GetChild(5) != null)
        {

            Debug.Log("does exist");
            Transform cinv = collision.transform.GetChild(3).GetChild(5);
            for (int i = 0; i <= collision.transform.GetChild(3).GetChild(5).childCount-1; i++) 
            {
                Debug.Log(collision.transform.GetChild(3).GetChild(5).GetChild(i).name + ": " + i );

                if (collision.transform.GetChild(3).GetChild(5).GetChild(i).name != ("Highlight"))
                    Destroy(collision.transform.GetChild(3).GetChild(5).GetChild(i).gameObject);        /// HONESTLY, IDK HOW THIS WORKED, WE SHOULD PROLLY FIX THIS LATER AYAAYAYAY
            }
                
            //Destroy(cinv.GetComponent<ItemGrid>());
            //cinv.gameObject.AddComponent(typeof(ItemGrid)) as ItemGrid;
            //cinv.gameObject.AddComponent<ItemGrid>();

        }
    }

    private void FixedUpdate()
    {
        //playerIn = Physics.SphereCast(this.gameObject.transform.position, checkRadius, -transform.forward, out hit, checkRadius, playerLayerMask);

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
        if (inChest) 
        {
            chestUI.SetActive(inChest);
        }
    }
}
