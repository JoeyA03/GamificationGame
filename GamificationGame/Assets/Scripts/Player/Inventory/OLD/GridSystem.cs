using UnityEngine;
using UnityEngine.UI;
using System;


public class GridSystem<GridObject>
{
    private int width;
    private int height;
    public float cellSize;
    public GridObject[,] gridArray;
    //public GameObject[,] DebugGrid;
    //public GameObject[,] DebugGridArrayText;
    //public Image[,] ImageGrid;

    public GridSystem(int width, int height)
    {
        this.width = width;
        this.height = height;
        // this.cellSize = cellSize;

        gridArray = new GridObject[width, height];

        // DebugGridArray = new int[width, height];

        // for(int x = 0; x < gridArray.GetLength(0); x++)
        // {
        //     for(int y = 0; y < gridArray.GetLength(1); y++)
        //     {
        //             GameObject point1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //             point1.transform.position = Vector3(x,y) * cellSize;

        //             // point1.AddComponent<DebugDestory>();
        //             Destroy(point1.GetComponent<SphereCollider>());
        //     }

        // }

    }

    public GridSystem(int width, int height, float cellSize, Sprite imageSprite, Transform t, Func<GridSystem<GridObject>, int, int, GameObject, GridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new GridObject[width, height];
        //DebugGrid = new GameObject[width, height];

        //DebugGrid = new GameObject[width, height];
        //DebugGridArrayText = new GameObject[width, height];
        // ImageGrid = new Image[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y, new GameObject());
            }
        }

        bool _debugSet = false;

        if (_debugSet) 
        {
            GameObject[,] DebugGrid;
            GameObject[,] DebugGridArrayText;

            DebugGrid = new GameObject[width, height];
            DebugGridArrayText = new GameObject[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    
                    DebugGrid[x, y] = new GameObject($"{x},{y}");
                    DebugGridArrayText[x, y] = new GameObject($"text {x}, {y}");

                    DebugGrid[x, y].AddComponent<Image>();
                    DebugGrid[x, y].AddComponent<InventorySlot>();
                    DebugGrid[x, y].GetComponent<Image>().sprite = imageSprite;

                    DebugGrid[x, y].transform.SetParent(t, true);
                    DebugGridArrayText[x, y].transform.SetParent(DebugGrid[x, y].transform, true);

                    DebugGrid[x, y].GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize, cellSize);
                    DebugGrid[x, y].GetComponent<RectTransform>().pivot = new Vector2(0, 0);
                    DebugGrid[x, y].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y) * (cellSize);


                    DebugGridArrayText[x, y].AddComponent<Text>().text = gridArray[x, y].ToString();
                    DebugGridArrayText[x, y].GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                    DebugGridArrayText[x, y].GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                    DebugGridArrayText[x, y].GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize, cellSize);

                    //Debug.DrawLine(getWorldSize(x, y), getWorldSize(x, y + 1), Color.white);
                    //Debug.DrawLine(getWorldSize(x, y), getWorldSize(x + 1, y), Color.white);
                }
            }
        }
    }

    public int GetWidth() 
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public Vector3 getCanvasPosition(int x, int y)
    {
        return new Vector2(x,y);
    }

    public void GetXY(Vector2 canvasPosition, out int x, out int y) 
    {
        x = Mathf.FloorToInt(canvasPosition.x);
        y = Mathf.FloorToInt(canvasPosition.y);
    }

    public void SetValue(int x, int y)
    {
        if( x >= 0 && y >= 0 && x < width && y < height)
        {
            //gridArray[x, y] = 50;
            //DebugGridArrayText[x, y].GetComponent<Text>().text = "50";
            // gridArray[x, y] = 5;
        }
    }

    public void SetValueReset(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            //gridArray[x, y] = 0;
            //DebugGridArrayText[x, y].GetComponent<Text>().text = "0";
            // gridArray[x, y] = 5;
        }
    }

    
}


