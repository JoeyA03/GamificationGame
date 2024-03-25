using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestGrid : MonoBehaviour
{
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


    public void OpenChest()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(8, 8);

        //InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        CreateRandomItem(5, 2);
        CreateRandomItem(2, 2);
        CreateRandomItem(1, 6);

        //PlaceItem(inventoryItem, 5, 2);

        //inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        //PlaceItem(inventoryItem, 2, 2);

        //inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        //PlaceItem(inventoryItem, 1, 6);

    }
    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    private void CreateRandomItem(int posX, int posY)
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();


        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);

        PlaceItem(inventoryItem, posX, posY);
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null) { return null; }

        ClearGridReference(toReturn);

        return toReturn;

    }

    private void ClearGridReference(InventoryItem item)
    {
        for (int i = 0; i < item.WIDTH; i++)
        {
            for (int j = 0; j < item.HEIGHT; j++)
            {
                inventoryItemSlot[item.onGridPositionX + i, item.onGridPositionY + j] = null;
            }
        }
    }



    internal InventoryItem GetItem(int x, int y)
    {
        return inventoryItemSlot[x, y];
    }

    public Vector2Int GetGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        int height = (int)(tileSizeHeight - itemToInsert.HEIGHT + 1);
        int width = (int)(tileSizeWidth - itemToInsert.WIDTH + 1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckAvailableSpace(x, y, itemToInsert.WIDTH, itemToInsert.HEIGHT) == true)
                {
                    return new Vector2Int(x, y);
                }
            }

        }

        return null;
    }

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        if (BoundaryCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT) == false)
        {
            return false;
        }

        if (OverlapItem(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            ClearGridReference(overlapItem);

        }

        PlaceItem(inventoryItem, posX, posY);

        return true;
    }

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform rt = inventoryItem.GetComponent<RectTransform>();
        rt.SetParent(this.rectTransform);

        for (int x = 0; x < inventoryItem.WIDTH; x++)
        {
            for (int y = 0; y < inventoryItem.HEIGHT; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        Vector2 pos = CalculatePositionOnGrid(inventoryItem, posX, posY);

        //rt.localPosition = pos;
        rt.localPosition = pos;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY)
    {
        Vector2 pos = new Vector2();

        pos.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.WIDTH / 2;
        pos.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.HEIGHT / 2);
        return pos;
    }

    private bool OverlapItem(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    if (overlapItem == null)
                    {
                        overlapItem = inventoryItemSlot[posX + x, posY + y];
                    }
                    else
                    {
                        if (overlapItem != inventoryItemSlot[posX + x, posY + y])
                        {
                            return false;
                        }
                    }

                }
            }

        }

        return true;
    }

    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    return false;


                }
            }

        }

        return true;
    }

    bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0)
        {
            return false;
        }

        if (posX >= tileSizeWidth || posY >= tileSizeHeight)
        {
            return false;
        }

        return true;
    }

    public bool BoundaryCheck(int posX, int posY, int width, int height)
    {
        if (PositionCheck(posX, posY) == false) { return false; }

        posX += width - 1;
        posY += height - 1;
        //posX += width;
        //posY += height;

        if (PositionCheck(posX, posY) == false) { return false; }

        return true;


    }
}
