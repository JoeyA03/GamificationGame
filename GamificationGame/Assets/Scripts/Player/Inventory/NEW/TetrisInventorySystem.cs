using System.Collections.Generic;
using UnityEngine;

public class TetrisInventorySystem : MonoBehaviour
{
    [HideInInspector]
    private ItemGrid selectedItemGrid;
    public ItemGrid SelectedItemGrid { get => selectedItemGrid; set { selectedItemGrid = value; inventoryHighlight.SetParent(value); } }

    InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rt;

    public List<ItemData> items;
    public GameObject itemPrefab;
    public Transform canvasTransform;

    InventoryHighlight inventoryHighlight;
    InventoryItem itemToHighlight;

    Vector2Int oldPosition;


    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    public void Update()
    {
        ItemDrag();


        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    if (selectedItem == null)
        //    {
        //        CreateRandomItem();
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.E)) 
        //{
        //    InsertRandomItem();
        //}

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            RotateItem();
        }

        if (selectedItemGrid == null) 
        {
            inventoryHighlight.Show(false);
            return; 
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(selectedItemGrid.GetGridPosition(Input.mousePosition));
            LeftMouseButtonPress();
        }

        //Debug.Log(selectedItemGrid.GetGridPosition(Input.mousePosition));
    }

    private void RotateItem()
    {
        if (selectedItem == null) { return; }
        selectedItem.Rotate();
    }

    private void InsertRandomItem()
    {
        if(selectedItem == null) { return; }

        CreateRandomItem();
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);

    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);

        if (posOnGrid == null) { return; }

        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);


    }

    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (oldPosition == positionOnGrid) { return; }
        
        oldPosition = positionOnGrid; 
        if(selectedItem == null) 
        {
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);
            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.setSize(itemToHighlight);
                //inventoryHighlight.SetParent(selectedItemGrid);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else 
            {
                inventoryHighlight.Show(false);
            }
        }
        else 
        {
            inventoryHighlight.Show(selectedItemGrid.BoundaryCheck(positionOnGrid.x, positionOnGrid.y, selectedItem.WIDTH, selectedItem.HEIGHT));
            inventoryHighlight.setSize(selectedItem);
            //inventoryHighlight.SetParent(selectedItemGrid);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        rt = inventoryItem.GetComponent<RectTransform>();
        rt.SetParent(canvasTransform);
        rt.SetAsLastSibling();

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;
        }

        return selectedItemGrid.GetGridPosition(position);
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (complete) 
        {
            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rt = selectedItem.GetComponent<RectTransform>();
                rt.SetAsLastSibling();
            }
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rt = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void ItemDrag()
    {
        if (selectedItem != null)
        {
            rt.position = Input.mousePosition;
        }
    }
}
