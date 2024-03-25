using UnityEngine;
using System.Collections.Generic;
using System.IO;

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
    public List<ItemData> itemsScrap;
    public List<ItemData> itemsFood;
    public List<ItemData> itemsMaterials;
    public List<ItemData> itemsMedicine;

    public List<ItemData> chestItems;

    private int minimumAmount;
    private int maximumAmount;

    private int amount;

    private bool food;
    private bool materials;
    private bool medicine;
    private bool scrap;

    private int minCountFood;
    private int minCountMats;
    private int minCountMeds;
    private int minCountScrap;

    private int maxCountFood;
    private int maxCountMats;
    private int maxCountMeds;
    private int maxCountScrap;

    public bool itemsMade = false;

    

    private void Start()
    {
        boxCollider = GetComponent<Collider>();

        //amount = UnityEngine.Random.Range(minimumAmount,maximumAmount);

    }

    private void FixedUpdate()
    {
        playerIn = Physics.SphereCast(this.gameObject.transform.position, checkRadius, -transform.forward, out hit, checkRadius, playerLayerMask);

    }
    public List<ItemData> CreateItems()
    {
        if(food == true)
        {
            amount = UnityEngine.Random.Range(minCountFood,maxCountFood);
            for(int i = 0; i < amount; i++)
            {
                int randomItem = UnityEngine.Random.Range(0,itemsFood.Count);
                chestItems.Add(itemsFood[randomItem]);
            }
        }
        if(materials == true)
        {
            amount = UnityEngine.Random.Range(minCountMats,maxCountMats);
            for(int i = 0; i < amount; i++)
            {
                int randomItem = UnityEngine.Random.Range(0,itemsMaterials.Count);
                chestItems.Add(itemsMaterials[randomItem]);
            }
        }
        if(medicine == true)
        {
            amount = UnityEngine.Random.Range(minCountMeds,maxCountMeds);
            for(int i = 0; i < amount; i++)
            {
                int randomItem = UnityEngine.Random.Range(0,itemsMedicine.Count);
                chestItems.Add(itemsMedicine[randomItem]);
            }
        }
        if(scrap == true)
        {
            amount = UnityEngine.Random.Range(minCountScrap,maxCountScrap);
            for(int i = 0; i < amount; i++)
            {
                int randomItem = UnityEngine.Random.Range(0,itemsScrap.Count);
                chestItems.Add(itemsScrap[randomItem]);
            }
        }
        itemsMade = true;
        return chestItems;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {   
            Debug.Log("yessssirr");
            TetrisInventorySystem.Instance.inChestRange = true;
            TetrisInventorySystem.Instance.currentChest = this.gameObject.GetComponent<ChestSystem>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TetrisInventorySystem.instance.inChestRange = false;
            TetrisInventorySystem.Instance.currentChest = null;
        }
    }





    //Chest tool setting options
    public void SetResourceSelections(bool[] resourceType)
    {
        if(resourceType[0] == true)
        {
            food = true;
        }
        if(resourceType[1] == true)
        {
            materials = true;
        }
        if(resourceType[2] == true)
        {
            medicine = true;
        }
        if(resourceType[3] == true)
        {
            scrap = true;
        }
        if(resourceType[0] == false)
        {
            food = false;
        }
        if(resourceType[1] == false)
        {
            materials = false;
        }
        if(resourceType[2] == false)
        {
            medicine = false;
        }
        if(resourceType[3] == false)
        {
            scrap = false;
        }
    }
    public void SetMinCounts(int[] minCount)
    {
        minCountFood = minCount[0];
        minCountMats = minCount[1];
        minCountMeds = minCount[2];
        minCountScrap = minCount[3];
    }
    public void SetMaxCounts(int[] maxCount)
    {
        maxCountFood = maxCount[0];
        maxCountMats = maxCount[1];
        maxCountMeds = maxCount[2];
        maxCountScrap = maxCount[3];
    }
    public void SetPercentChance(int[] percentToSpawn)
    {

    }
    
    
  









}
