using UnityEngine;
using System.Collections.Generic;

public class ChestSystem : MonoBehaviour
{
    public bool playerIn;
    public bool inChest;
    public float checkRadius;
    public LayerMask playerLayerMask;

    Collider boxCollider;
    RaycastHit hit;

    public GameObject chestUI;
    public const float tileSizeWidth = 32;
    public const float tileSizeHeight = 32;

    InventoryItem[,] inventoryItemSlot;
    //RectTransform rectTransform;
    RectTransform rectTransform;


    public int sizeWidth, sizeHeight;


    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    public GameObject itemPrefab;
    public List<ItemData> items;



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

    private void CreateItem(int posX, int posY)
    {

    }

    



    //Chest tool setting options
    public void SetResourceSelections(bool[] resourceType)
    {

    }
    public void SetMinCounts(int[] minCount)
    {

    }
    public void SetMaxCounts(int[] maxCount)
    {

    }
    public void SetPercentChance(int[] percentToSpawn)
    {

    }










}
