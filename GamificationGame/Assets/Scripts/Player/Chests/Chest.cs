using UnityEngine;
using UnityEngine.EventSystems;

public class Chest : MonoBehaviour
{
    public Player player;
    //public GameObject chestInventoryCanvas;
    private RectTransform playerInventoryRectTransform;
    public GameObject chestUI;
    private bool inInventory = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InventorySet();
        }
    }

    void OnMouseDown()
    {
        ToggleInventoriesVisibility();
    }
    

    void ToggleInventoriesVisibility()
    {
        player.InventorySet();
        MovePlayerInventoryToLeft();
    }
    void MovePlayerInventoryToLeft()
    {
        // Adjust the position of the player inventory to the left
        playerInventoryRectTransform = GameObject.FindWithTag("Player").gameObject.transform.Find("Inventory").gameObject.transform.Find("Border").GetComponent<RectTransform>();
        inInventory = !inInventory;
        chestUI.SetActive(inInventory);
        playerInventoryRectTransform.anchoredPosition = new Vector2(-300f, 0);

    }
    void InventorySet()
    {
        if(inInventory)
        {
            inInventory = !inInventory;
            chestUI.SetActive(inInventory);
        }
    }
}
